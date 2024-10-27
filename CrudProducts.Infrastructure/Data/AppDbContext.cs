using CrudProducts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CrudProducts.Infrastructure.Data
{
    // Clase que representa el contexto de la base de datos, que hereda de DbContext
    public class AppDbContext : DbContext
    {
        // Constructor que recibe las opciones de configuración del contexto
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Conjunto de entidades de tipo Product, que representa la tabla de productos en la base de datos
        public DbSet<Product> Products { get; set; }

        // Método para configurar el modelo de datos y las entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura la entidad Product para que use la tabla "products" en la base de datos
            modelBuilder.Entity<Product>().ToTable("products");
        }
    }
}
