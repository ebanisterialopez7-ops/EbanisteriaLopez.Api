using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EbanisteriaLopez.Api.Models
{
    public class Venta
    {
        public int VentaId { get; set; }
        public string CorreoUsuario { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;
        public string? UrlVoucher { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string EstadoEntrega { get; set; } = "Pedido recibido";

        // Relación 1:N con VentaItem
        public List<VentaItem> Items { get; set; } = new();
    }

    public class VentaItem
    {
        public int VentaItemId { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        [NotMapped]
        public decimal SubTotal => Cantidad * PrecioUnitario; // Para cálculos rápidos
    }
}