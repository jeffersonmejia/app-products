using CrudProductos.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudProductos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(c => c.Nombre).HasMaxLength(80).IsRequired();
                entity.Property(c => c.Descripcion).HasMaxLength(200).IsRequired();
            });
        }
    }
}
