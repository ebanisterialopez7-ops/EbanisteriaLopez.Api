using System.Collections.Generic;

namespace EbanisteriaLopez.Api.Models
{
    public class CrearVentaDto
    {
        public string CorreoUsuario { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;

        // Eliminamos Total ya que lo calcularemos en el servidor
        // public decimal Total { get; set; }

        public List<CrearVentaItemDto> Items { get; set; } = new();
    }

    public class CrearVentaItemDto
    {
        public string NombreProducto { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
    }
}