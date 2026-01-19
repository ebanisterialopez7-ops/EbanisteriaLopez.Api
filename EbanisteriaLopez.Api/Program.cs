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

// =====================
// Registrar servicios de Autenticación y Autorización
// =====================
builder.Services.AddAuthorization(); // <- obligatorio si usas UseAuthorization
// Si no vas a usar autenticación, puedes comentar lo siguiente:
// builder.Services.AddAuthentication("Bearer")
//     .AddJwtBearer("Bearer", options =>
//     {
//         options.Authority = "https://tu-servidor-de-auth";
//         options.Audience = "ebanisteria-api";
//     });

var app = builder.Build();

// =====================
// Middleware
// =====================
app.UseHttpsRedirection();

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EbanisteriaLopez API V1");
    c.RoutePrefix = string.Empty;
});

// Si agregas autenticación
// app.UseAuthentication(); // debe ir antes de UseAuthorization

app.UseAuthorization(); // requiere AddAuthorization()

// Mapear controladores
app.MapControllers();

// Endpoint de prueba
app.MapGet("/health", () => Results.Ok(new { status = "Healthy" }));

app.Run();