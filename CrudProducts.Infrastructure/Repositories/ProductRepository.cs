using CrudProducts.Application.Interfaces;
using CrudProducts.Domain.Entities;
using CrudProducts.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudProducts.Infrastructure.Repositories
{
    // Clase que implementa la interfaz IProductRepository para gestionar operaciones sobre productos
    public class ProductRepository : IProductRepository
    {
        private AppDbContext _context; // Contexto de la base de datos

        // Constructor que recibe el contexto de la base de datos
        public ProductRepository(AppDbContext context)
        {
            _context = context; // Inicializa el contexto
        }

        // Método para obtener todos los productos de la base de datos
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                // Obtiene la lista de productos de forma asíncrona
                var products = await _context.Products.ToListAsync();

                return products; // Retorna la lista de productos
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción y lanza una nueva 
                throw new Exception("Error al obtener todos los productos", ex);
            }
        }

        // Método para obtener un producto por su ID
        public async Task<Product> GetByIdAsync(Guid id)
        {
            try
            {
                // Busca el producto por su ID de forma asíncrona
                var product = await _context.Products.FirstOrDefaultAsync(p => p.id == id);
                return product; // Retorna el producto encontrado o null si no existe
            }
            catch (Exception ex)
            {
                // Lanza una nueva excepción 
                throw new Exception("Error al obtener el producto por ID", ex);
            }
        }

        // Método para agregar un nuevo producto a la base de datos
        public async Task AddAsync(Product product)
        {

            try
            {
                // Agrega el producto al contexto
                await _context.Products.AddAsync(product);
                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Captura excepciones relacionadas con la actualización de la base de datos
                throw new Exception("Error al agregar el producto a la base de datos.", dbEx);
            }
            catch (Exception ex)
            {
                // Captura cualquier otra excepción y lanza una nueva
                throw new Exception("Error al crear el producto", ex);
            }
        }

        // Método para actualizar un producto existente en la base de datos
        public async Task UpdateAsync(Product product)
        {

            try
            {
                // Actualiza el producto en el contexto
                _context.Products.Update(product);
                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Captura excepciones relacionadas con la actualización de la base de datos
                throw new Exception("Error al actualizar el producto a la base de datos.", dbEx);
            }
            catch (Exception ex)
            {
                // Captura cualquier otra excepción y lanza una nueva
                throw new Exception("Error al actualizar el producto", ex);
            }
        }

        // Método para eliminar un producto por su ID
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                // Busca el producto por su ID
                var product = await GetByIdAsync(id);


                // Elimina el producto del contexto
                _context.Products.Remove(product);
                // Guarda los cambios en la base de datos
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Captura excepciones relacionadas con la actualización de la base de datos
                throw new Exception("Error al eliminar el producto de la base de datos.", dbEx);
            }
            catch (Exception ex)
            {
                // Captura cualquier otra excepción y lanza una nueva
                throw new Exception("Error al eliminar el producto", ex);
            }
        }
    }
}
