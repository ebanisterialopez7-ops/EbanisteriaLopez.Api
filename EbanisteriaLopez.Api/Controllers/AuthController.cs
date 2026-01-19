using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EbanisteriaLopez.Api.Data;
using EbanisteriaLopez.Api.Models;
using System.Security.Cryptography;

namespace EbanisteriaLopez.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == login.Email);

            if (user == null)
                return Unauthorized("Usuario no encontrado");

            if (!VerifyPassword(login.Password, user.PasswordHash))
                return Unauthorized("Contraseña incorrecta");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        // =====================
        // Métodos privados
        // =====================
        private bool VerifyPassword(string password, string? storedHash)
        {
            if (string.IsNullOrEmpty(storedHash)) return false;

            // Para ASP.NET Identity: PasswordHash es Base64 de SHA256 + Salt codificado.
            // Aquí usamos la librería Identity para comparar
            var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Usuario>();
            var result = passwordHasher.VerifyHashedPassword(new Usuario(), storedHash, password);
            return result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
        }

        private string GenerateJwtToken(Usuario user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}