using EcoScale.src.Models;
using EcoScale.src.Models.Abstract;
using EcoScale.src.Models.Notifications;
using Microsoft.EntityFrameworkCore;

namespace EcoScale.src.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Moderador> Moderadores { get; set; }
        public DbSet<Questionario> Questionarios { get; set; }
        public DbSet<EmailConfirmacao> EmailConfirmacoes { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }
        public DbSet<ResponsavelEmpresa> ResponsavelEmpresa { get; set; }
        public DbSet<Criterio> Criterios { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ItemAvaliado> ItensAvaliados { get; set; }
        public DbSet<Tema> Temas { get; set; }
        public DbSet<Planilha> Planilhas { get; set; }
        public DbSet<AreaPlanilha> AreasPlanilha { get; set; }
        public DbSet<TemaPlanilha> TemasPlanilha { get; set; }
        public DbSet<CriterioPlanilha> CriteriosPlanilha { get; set; }
        public DbSet<ItemAvaliadoPlanilha> ItensAvaliadosPlanilha { get; set; }
        public DbSet<ReqAvaliacaoModel> ReqAvaliacoes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var DatabaseSenha = Environment.GetEnvironmentVariable("DB_PSS") ?? throw new Exception("Nome do banco de dados não está no appsettings.json");
            optionsBuilder.UseNpgsql($"Host=db.jxfltssyguhzmkrlggfr.supabase.co;Port=5432;Database=postgres;Username=postgres;Password={DatabaseSenha}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .ToTable("usuario");
            modelBuilder.Entity<Empresa>()
                .ToTable("empresa");
            modelBuilder.Entity<Moderador>()
                .ToTable("moderador");

            modelBuilder.Entity<Planilha>().ToTable("planilha");
            modelBuilder.Entity<Questionario>().ToTable("questionario");
            modelBuilder.Entity<AreaPlanilha>().ToTable("area_planilha");
            modelBuilder.Entity<TemaPlanilha>().ToTable("tema_planilha");
            modelBuilder.Entity<CriterioPlanilha>().ToTable("criterio_planilha");
            modelBuilder.Entity<ItemAvaliadoPlanilha>().ToTable("item_avaliado_planilha");
        }
    }
}