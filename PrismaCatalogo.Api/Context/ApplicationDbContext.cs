using Microsoft.EntityFrameworkCore;
using dotenv.net;
using PrismaCatalogo.Api.Models;

namespace PrismaCatalogo.Api.Context
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
            //modelBuilder.Entity<Foto>(f =>
            //{
            //    f.HasKey(t => t.Id);
            //});

            modelBuilder.Entity<Tamanho>(e => {
                e.HasKey(t => t.Id);
                e.HasIndex(t => t.Nome).IsUnique(true);
            });

            modelBuilder.Entity<Cor>(c =>
            {
                c.HasKey(t => t.Id);
                c.HasIndex(t => t.Nome).IsUnique(true);
            });

            modelBuilder.Entity<Categoria>(e => {
                e.HasIndex(c => new { c.IdPai, c.Nome }).IsUnique(true);
            });

            modelBuilder.Entity<Categoria>()
                .HasMany(e => e.CategoriasFilhas)
                .WithOne(e => e.CategoriaPai)
                .HasForeignKey(e => e.IdPai)
                .HasPrincipalKey(e => e.Id);

            modelBuilder.Entity<Produto>(c =>
            {
                c.HasKey(t => t.Id);
                c.HasIndex(t => t.Nome).IsUnique(true);
            });

            modelBuilder.Entity<Produto>()
                .HasMany(p => p.ProdutosFilhos)
                .WithOne(p => p.Produto)
                .HasForeignKey(f => f.ProdutoId);

            modelBuilder.Entity<Produto>()
                .HasMany(p => p.Fotos)
                .WithOne(p => p.Produto)
                .HasForeignKey(f => f.ProdutoId);

            modelBuilder.Entity<ProdutoFilho>(c =>
            {
                c.HasKey(t => t.Id);
                //c.HasIndex(t => t.Nome).IsUnique(true);
                c.HasIndex(p => new { p.ProdutoId, p.CorId, p.TamanhoId }).IsUnique(true);
                c.Property(t => t.Nome).IsRequired(false);
            });

            modelBuilder.Entity<ProdutoFilho>()
                .HasOne(f => f.Cor)
                .WithMany(p => p.ProdutosFilhos)
                .HasForeignKey(f => f.CorId);

            modelBuilder.Entity<ProdutoFilho>()
                .HasOne(f => f.Tamanho)
                .WithMany(p => p.ProdutosFilhos)
                .HasForeignKey(f => f.TamanhoId);

            modelBuilder.Entity<ProdutoFilho>()
                .HasMany(p => p.Fotos)
                .WithOne(p => p.ProdutoFilho)
                .HasForeignKey(f => f.ProdutoFilhoId);

            modelBuilder.Entity<ProdutoFoto>(c =>
            {
                c.HasKey(t => t.Id);
            });

            modelBuilder.Entity<ProdutoFilhoFoto>(c =>
            {
                c.HasKey(t => t.Id);
            });

            modelBuilder.Entity<Usuario>(u =>
            {
                u.HasKey(t => t.Id);
                u.HasIndex(t => t.NomeUsuario).IsUnique(true);
                u.Property(t => t.Nome).IsRequired(true);
                u.Property(t => t.Senha).IsRequired(true);
            });
        }

        public DbSet<Tamanho> Tamanhos { get; set; }
        public DbSet<Cor> Cores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProdutoFilho> ProdutosFilhos { get; set; }
        public DbSet<ProdutoFoto> ProdutosFoto { get; set; }
        public DbSet<ProdutoFilhoFoto> produtoFilhoFotos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
