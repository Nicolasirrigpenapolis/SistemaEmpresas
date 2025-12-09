using Microsoft.EntityFrameworkCore;
using SistemaEmpresas.Models;

namespace SistemaEmpresas.Data
{
    public class TenantDbContext : DbContext
    {
        public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índice único para domínio
            modelBuilder.Entity<Tenant>()
                .HasIndex(t => t.Dominio)
                .IsUnique();

            // Seed inicial - Empresas do sistema
            modelBuilder.Entity<Tenant>().HasData(
                new Tenant
                {
                    Id = 1,
                    Nome = "Irrigação Penápolis",
                    Dominio = "irrigacao",
                    ConnectionString = "Server=DESKTOP-CHS14C0\\SQLIRRIGACAO;Database=IRRIGACAO;Trusted_Connection=True;TrustServerCertificate=True;",
                    Ativo = true
                },
                new Tenant
                {
                    Id = 2,
                    Nome = "Chinellato Transportes",
                    Dominio = "chinellato",
                    ConnectionString = "Server=DESKTOP-CHS14C0\\SQLIRRIGACAO;Database=ChinellatoTransportes;Trusted_Connection=True;TrustServerCertificate=True;",
                    Ativo = true
                }
            );
        }
    }
}
