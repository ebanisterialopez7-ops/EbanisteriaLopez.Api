using EbanisteriaLopez.Api.Data;
using EbanisteriaLopez.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EbanisteriaLopez.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Ventas
        [HttpPost]
        public async Task<IActionResult> CrearVenta([FromBody] CrearVentaDto ventaDto)
        {
            if (ventaDto.Items == null || !ventaDto.Items.Any())
                return BadRequest("La venta debe contener al menos un producto.");

            var total = ventaDto.Items.Sum(i => i.Cantidad * i.PrecioUnitario);

            var venta = new Venta
            {
                CorreoUsuario = ventaDto.CorreoUsuario,
                NombreCliente = ventaDto.NombreCliente,
                Telefono = ventaDto.Telefono,
                Direccion = ventaDto.Direccion,
                MetodoPago = ventaDto.MetodoPago,
                UrlVoucher = ventaDto.UrlVoucher,
                Fecha = DateTime.Now,
                Total = total,
                EstadoEntrega = "Pedido recibido",
                Items = ventaDto.Items.Select(i => new VentaItem
                {
                    ProductoId = i.ProductoId,
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario
                }).ToList()
            };

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return Ok(new { venta.VentaId, mensaje = "Venta registrada correctamente", total = venta.Total });
        }

        // GET: api/Ventas
        [HttpGet]
        public async Task<IActionResult> ObtenerVentas()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Items)
                .ToListAsync();

            return Ok(ventas);
        }

        // =======================
        // DTOs internos del controller
        // =======================
        public class CrearVentaDto
        {
            public string CorreoUsuario { get; set; } = string.Empty;
            public string NombreCliente { get; set; } = string.Empty;
            public string Telefono { get; set; } = string.Empty;
            public string Direccion { get; set; } = string.Empty;
            public string MetodoPago { get; set; } = string.Empty;
            public string? UrlVoucher { get; set; }
            public List<CrearVentaItemDto> Items { get; set; } = new();
        }

        public class CrearVentaItemDto
        {
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
        }
    }
}