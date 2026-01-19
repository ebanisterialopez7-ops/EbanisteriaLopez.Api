using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EbanisteriaLopez.Api.Data;
using EbanisteriaLopez.Api.Models;

namespace EbanisteriaLopez.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Usuarios/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Correo y contraseña son obligatorios.");

            var exists = await _context.Usuarios.AnyAsync(u => u.Email == request.Email);
            if (exists) return BadRequest("El usuario ya existe.");

            var usuario = new Usuario
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email
            };

            var hasher = new PasswordHasher<Usuario>();
            usuario.PasswordHash = hasher.HashPassword(usuario, request.Password);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { usuario.Email });
        }

        // POST: api/Usuarios/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Correo y contraseña son obligatorios.");

            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
                return Unauthorized("Correo o contraseña incorrectos.");

            var hasher = new PasswordHasher<Usuario>();
            var result = hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, request.Password);

            if (result != PasswordVerificationResult.Success)
                return Unauthorized("Correo o contraseña incorrectos.");

            // Devuelve solo correo para no exponer hash
            return Ok(new { usuario.Email });
        }
    }

    // DTOs
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}