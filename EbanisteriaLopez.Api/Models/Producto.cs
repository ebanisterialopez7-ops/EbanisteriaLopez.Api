
using System.Collections.Generic; // <-- necesario para List<>
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbanisteriaLopez.Api.Models;

public class Producto
{
    [Key]
    public int ProductoId { get; set; }

    [Required, MaxLength(255)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(50)]
    public string Color { get; set; } = string.Empty;

    [StringLength(100)]
    public string Material { get; set; } = string.Empty;

    [StringLength(200)]
    public string Dimensiones { get; set; } = string.Empty;

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Precio { get; set; }

    public int CategoriaId { get; set; }
    public int EstadoProductoId { get; set; }

    public Categoria? Categoria { get; set; }
    public EstadoProducto? EstadoProducto { get; set; }
    public ProductoDetalle? Detalle { get; set; }

    // =====================
    // Relación 1 a muchos con ImagenProducto
    // =====================
    public List<ImagenProducto> Imagenes { get; set; } = new();
}