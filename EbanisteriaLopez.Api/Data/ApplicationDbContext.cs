using EbanisteriaLopez.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EbanisteriaLopez.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<EstadoProducto> EstadoProductos => Set<EstadoProducto>();
        public DbSet<ProductoDetalle> ProductoDetalles => Set<ProductoDetalle>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
        public DbSet<ImagenProducto> ImagenesProducto => Set<ImagenProducto>();

        // Aquí están las entidades de ventas
        public DbSet<Venta> Ventas => Set<Venta>();
        public DbSet<VentaItem> VentaItems => Set<VentaItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Nombres exactos de tablas
            modelBuilder.Entity<Producto>().ToTable("Producto");
            modelBuilder.Entity<Categoria>().ToTable("Categoria");
            modelBuilder.Entity<EstadoProducto>().ToTable("EstadoProducto");
            modelBuilder.Entity<ProductoDetalle>().ToTable("ProductoDetalle");
            modelBuilder.Entity<ImagenProducto>().ToTable("ImagenProducto");
            modelBuilder.Entity<Venta>().ToTable("Ventas");
            modelBuilder.Entity<VentaItem>().ToTable("VentaItems");

            // Relación Producto <-> ProductoDetalle
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Detalle)
                .WithOne(d => d.Producto)
                .HasForeignKey<ProductoDetalle>(d => d.ProductoId);

            // Relación Producto <-> ImagenProducto
            modelBuilder.Entity<Producto>()
                .HasMany(p => p.Imagenes)
                .WithOne(i => i.Producto)
                .HasForeignKey(i => i.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación Venta <-> VentaItem
            modelBuilder.Entity<Venta>()
                .HasMany(v => v.Items)
                .WithOne()
                .HasForeignKey("VentaId")
                .OnDelete(DeleteBehavior.Cascade);

            // Evitar ciclos en JSON
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}