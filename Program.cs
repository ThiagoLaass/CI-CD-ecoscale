using Serilog;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using DotNetEnv;
using System.Reflection;
using Microsoft.OpenApi.Models;
using EcoScale.src.Middlewares;
using EcoScale.src.Middlewares.Exceptions;
using EcoScale.src.Data;
using EcoScale.src.Services;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "🌱📐 EcoScale API", 
        Version = "v1",
        Description = "O endpoint da requisição não precisa estar em maisculo.\n" +
                      "Os endpoints da LLM são apenas para testes.\n",
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
        policy
            .WithOrigins("https://pmg-es-2025-1-ti4-3170100-pesquisa-bh-tec.onrender.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

// Configuração unificada da Autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? throw new ArgumentNullException("Jwt:Key");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"] ?? Environment.GetEnvironmentVariable("JWT_ISSUER"),
        ValidAudience = configuration["Jwt:Audience"] ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            throw new UnauthorizedException("Falha na autenticação do token JWT");
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            throw new UnauthorizedException("Token de autorização ausente ou inválido.");
        }
    };
});

// Configuração da Autorização para cada entidade
builder.Services.AddAuthorizationBuilder().AddPolicy("EmpresaPolicy", policy =>
{
    policy.RequireRole("Empresa", "Moderador");
});

builder.Services.AddAuthorizationBuilder().AddPolicy("ModeradorPolicy", policy =>
{
    policy.RequireRole("Moderador");
});


// Adicionando a conexão com o banco de dados
builder.Services.AddDbContext<AppDbContext>();

builder.Services
    .AddHttpClient<LLMService>(client =>
    {
        client.BaseAddress = new Uri("http://localhost:5050/");
        client.Timeout     = TimeSpan.FromSeconds(30);
    });

builder.Services.AddControllers();

builder.Logging.ClearProviders();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Pipeline de requisição
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    var swaggerUrl = "http://localhost:5051/swagger/index.html";
    Log.Information("Swagger UI available at {SwaggerUrl}", swaggerUrl);
}
else 
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}
app.UseMiddleware<ErrorHandlingMiddleware>();

// Log primary request info
app.Use((context, next) =>
{
    Log.Information("Request from: {Origin}", context.Request.Headers.Origin);
    return next();
});

app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

Log.Information("Server starting and listening on {Url}", app.Urls.FirstOrDefault());
app.Run();