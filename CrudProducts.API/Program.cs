using CrudProducts.Application.Interfaces; 
using CrudProducts.Application.Commands; 
using CrudProducts.Infrastructure.Data;
using CrudProducts.Infrastructure.Repositories; 
using Microsoft.EntityFrameworkCore; 
using MediatR; 
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args); // Crear un nuevo builder para la aplicaci�n web

// Configuraci�n de DotNetEnt
Env.Load();
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") 
                        ?? builder.Configuration.GetConnectionString("DefaultConnection");


// Configuraci�n de DbContext para MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString)); // Usar MySQL y la cadena de conexi�n definida en appsettings

// Configuraci�n de MediatR para manejar comandos
builder.Services.AddMediatR(typeof(CreateProductCommandHandler).Assembly); // Registrar el manejador para crear productos
builder.Services.AddMediatR(typeof(PatchProductCommandHandler).Assembly); // Registrar el manejador para actualizar productos
builder.Services.AddMediatR(typeof(DeleteProductCommandHandler).Assembly); // Registrar el manejador para eliminar productos

// Configuraci�n del repositorio
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // Registrar el repositorio de productos con el contenedor de inyecci�n de dependencias

// Configuraci�n de controladores
builder.Services.AddControllers(); // Agregar soporte para controladores

// Configuraci�n de SwaggerGen para la documentaci�n de la API
builder.Services.AddEndpointsApiExplorer(); // Permitir la exploraci�n de puntos finales
builder.Services.AddSwaggerGen(c =>
{
    // Configuraci�n de Swagger con informaci�n del documento
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "REST API de Productos con .NET y MySQL", Version = "v1" });
});



var app = builder.Build(); // Construir la aplicaci�n

// Configuraci�n de middleware
if (app.Environment.IsDevelopment()) // Verificar si est� en entorno de desarrollo
{
    app.UseDeveloperExceptionPage(); // Usar la p�gina de excepciones del desarrollador
}

app.UseRouting(); // Habilitar el enrutamiento
app.UseAuthorization(); // Habilitar la autorizaci�n

// Configuraci�n de SwaggerUI
app.UseSwagger(); // Habilitar Swagger
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Crud Products API v1"); // Establecer el endpoint para la documentaci�n de la API
    c.RoutePrefix = string.Empty; // Hacer que Swagger UI est� en la ra�z ("/")
});

// Mapeo de controladores
app.MapControllers(); // Mapear los controladores a las rutas

// Ejecutar la aplicaci�n
app.Run(); // Iniciar la aplicaci�n
