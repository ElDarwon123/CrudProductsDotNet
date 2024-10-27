using CrudProducts.Application.Interfaces; 
using CrudProducts.Application.Commands; 
using CrudProducts.Infrastructure.Data;
using CrudProducts.Infrastructure.Repositories; 
using Microsoft.EntityFrameworkCore; 
using MediatR; 
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args); // Crear un nuevo builder para la aplicación web

// Configuración de DotNetEnt
Env.Load();
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
                        ?? builder.Configuration.GetConnectionString("DefaultConnection");


// Configuración de DbContext para MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString)); // Usar MySQL y la cadena de conexión definida en appsettings

// Configuración de MediatR para manejar comandos
builder.Services.AddMediatR(typeof(CreateProductCommandHandler).Assembly); // Registrar el manejador para crear productos
builder.Services.AddMediatR(typeof(PatchProductCommandHandler).Assembly); // Registrar el manejador para actualizar productos
builder.Services.AddMediatR(typeof(DeleteProductCommandHandler).Assembly); // Registrar el manejador para eliminar productos

// Configuración del repositorio
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // Registrar el repositorio de productos con el contenedor de inyección de dependencias

// Configuración de controladores
builder.Services.AddControllers(); // Agregar soporte para controladores

// Configuración de SwaggerGen para la documentación de la API
builder.Services.AddEndpointsApiExplorer(); // Permitir la exploración de puntos finales
builder.Services.AddSwaggerGen(c =>
{
    // Configuración de Swagger con información del documento
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "REST API de Productos con .NET y MySQL", Version = "v1" });
});



var app = builder.Build(); // Construir la aplicación

// Configuración de middleware
if (app.Environment.IsDevelopment()) // Verificar si está en entorno de desarrollo
{
    app.UseDeveloperExceptionPage(); // Usar la página de excepciones del desarrollador
}

app.UseRouting(); // Habilitar el enrutamiento
app.UseAuthorization(); // Habilitar la autorización

// Configuración de SwaggerUI
app.UseSwagger(); // Habilitar Swagger
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Crud Products API v1"); // Establecer el endpoint para la documentación de la API
    c.RoutePrefix = string.Empty; // Hacer que Swagger UI esté en la raíz ("/")
});

// Mapeo de controladores
app.MapControllers(); // Mapear los controladores a las rutas

// Ejecutar la aplicación
app.Run(); // Iniciar la aplicación
