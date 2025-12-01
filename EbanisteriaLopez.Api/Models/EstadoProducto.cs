using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EbanisteriaLopez.Api.Models;

public class EstadoProducto
{
    [Key]
    public int EstadoProductoId { get; set; }

    [Required, MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    // Evita ciclos al serializar JSON
    [JsonIgnore]
    public ICollection<Producto>? Productos { get; set; }
}