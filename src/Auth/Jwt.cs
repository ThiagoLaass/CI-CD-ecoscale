using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace EcoScale.src.Auth
{
    public class Jwt {
        public string GenerateToken(string email, bool IsModerador)
        {
            var claims = new List<Claim> {
                new("Email", email)
            };
            
            if (IsModerador) {
                claims.Add(new Claim(ClaimTypes.Role, "Moderador"));
            }
            else{
                claims.Add(new Claim(ClaimTypes.Role, "Empresa"));
            }

            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                   ?? throw new ArgumentNullException("JWT_SECRET_KEY", "Chave JWT não configurada");
            var issuer    = Environment.GetEnvironmentVariable("JWT_ISSUER")
                   ?? throw new ArgumentNullException("JWT_ISSUER", "Issuer JWT não configurado");
            var audience  = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                   ?? throw new ArgumentNullException("JWT_AUDIENCE", "Audience JWT não configurado");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds 
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Obtém as declarações (claims) contidas no token JWT informado.
        /// Utiliza a classe JwtSecurityTokenHandler para processar o token e extrair suas declarações.
        /// </summary>
        /// <param name="token">O token JWT no formato string.</param>
        /// <returns>Uma coleção de objetos Claim extraídos do token.</returns>
        public IEnumerable<Claim> GetClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims;
        }

        public string GetSpecificClaim(string token, string claimName) {
            var claims = GetClaims(token);
            var claim = claims.FirstOrDefault(c => c.Type.Equals(claimName, StringComparison.OrdinalIgnoreCase));
            return claim?.Value ?? string.Empty;
        }

        public string GetToken(HttpContext context) {
            return context.Request.Headers.Authorization.ToString().Replace("Bearer ", "");        
        }
    }
}