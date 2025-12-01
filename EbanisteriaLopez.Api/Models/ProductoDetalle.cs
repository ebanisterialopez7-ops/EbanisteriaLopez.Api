using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization; // <- necesario para JsonIgnore

namespace EbanisteriaLopez.Api.Models
{
    public class ProductoDetalle
    {
        [Key]
        public int ProductoDetalleId { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "El material no puede exceder los 100 caracteres.")]
        public string? Material { get; set; }

        [StringLength(50, ErrorMessage = "El color no puede exceder los 50 caracteres.")]
        public string? Color { get; set; }

        [StringLength(200, ErrorMessage = "Las dimensiones no pueden exceder los 200 caracteres.")]
        public string? Dimensiones { get; set; }

        // Relación 1 a 1 con Producto
        [Required]
        public int ProductoId { get; set; }

        [JsonIgnore] // <-- Esto evita ciclos infinitos al serializar
        public Producto? Producto { get; set; }
    }
}