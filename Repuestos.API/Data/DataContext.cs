using Microsoft.EntityFrameworkCore;
using Repuestos.API.Models.Entities;

namespace Repuestos.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Marca> Marcas { get; set; }

        public DbSet<Repuesto> Repuestos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Marca>()
                .Property(m => m.Nombre)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Repuesto>()
                .Property(r => r.Codigo)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Repuesto>()
                .HasIndex(r => r.Codigo)
                .IsUnique();

            modelBuilder.Entity<Repuesto>()
                .Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Repuesto>()
                .Property(r => r.Precio)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Repuesto>()
                .HasOne(r => r.Marca)
                .WithMany(m => m.Repuestos)
                .HasForeignKey(r => r.MarcaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}