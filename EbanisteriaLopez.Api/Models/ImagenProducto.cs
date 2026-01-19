using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // <- necesario para JsonIgnore

namespace EbanisteriaLopez.Api.Models
{
    [Table("ImagenProducto")] // Nombre exacto de la tabla en PostgreSQL
    public class ImagenProducto
    {
        [Key]
        public int ImagenId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        [StringLength(255)]
        public string UrlImagen { get; set; } = string.Empty;

        [Required]
        public int Orden { get; set; }

        // Relación con Producto
        [JsonIgnore] // <- Evita ciclos infinitos al serializar
        public Producto? Producto { get; set; }
    }
}