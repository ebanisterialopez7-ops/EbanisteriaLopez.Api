using EbanisteriaLopez.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// =====================
// Conexión a Supabase PostgreSQL
// =====================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// =====================
// Servicios de API y Swagger
// =====================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EbanisteriaLopez API",
        Version = "v1"
    });
});

var app = builder.Build();

// =====================
// Middleware
// =====================

// Solo habilitamos HTTPS redirection si se requiere, pero Swagger funciona mejor con HTTP/HTTPS directo
app.UseHttpsRedirection();

// Swagger siempre visible en la raíz
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EbanisteriaLopez API V1");
    c.RoutePrefix = string.Empty; // Hace que Swagger esté en: https://localhost:<puerto>/
});

// Autorización (si no hay JWT/Identity, no afecta)
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

// Endpoint de prueba opcional
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

app.Run();