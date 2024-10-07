using Microsoft.EntityFrameworkCore;
using dotenv.net;
using PrismaCatalogo.Models;

namespace PrismaCatalogo.Context
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                DotEnv.Load();
                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tamanho>(e => {
                e.HasKey(t => t.Id);
                e.HasIndex(t => t.Nome).IsUnique(true);
            });

            modelBuilder.Entity<Categoria>(e => {
                e.HasIndex(c => new { c.IdPai, c.Nome }).IsUnique(true);

            });

            modelBuilder.Entity<Categoria>()
                .HasMany(e => e.CategoriasFilhas)
                .WithOne(e => e.CategoriaPai)
                .HasForeignKey(e => e.IdPai)
                .HasPrincipalKey(e => e.Id); 
        }

        public DbSet<Tamanho> Tamanhos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
    }
}
