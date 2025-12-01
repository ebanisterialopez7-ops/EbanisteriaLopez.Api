using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EbanisteriaLopez.Api.Models;

public class Categoria
{
    [Key]
    public int CategoriaId { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    // Evita ciclos al serializar JSON
    [JsonIgnore]
    public ICollection<Producto>? Productos { get; set; }
}