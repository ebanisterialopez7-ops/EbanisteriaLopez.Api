using Microsoft.EntityFrameworkCore;
using EbanisteriaLopez.Api.Models;
using System.Linq;

namespace EbanisteriaLopez.Api.Data;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Nombres exactos de tablas (para PostgreSQL)
        modelBuilder.Entity<Producto>().ToTable("Producto");
        modelBuilder.Entity<Categoria>().ToTable("Categoria");
        modelBuilder.Entity<EstadoProducto>().ToTable("EstadoProducto");
        modelBuilder.Entity<ProductoDetalle>().ToTable("ProductoDetalle");

        // Relación 1 a 1 Producto <-> ProductoDetalle
        modelBuilder.Entity<Producto>()
            .HasOne(p => p.Detalle)
            .WithOne(d => d.Producto)
            .HasForeignKey<ProductoDetalle>(d => d.ProductoId);

        // Evitar ciclos en JSON
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}