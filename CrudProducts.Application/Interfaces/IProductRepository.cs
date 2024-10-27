using CrudProducts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudProducts.Application.Interfaces
{
    // Interfaz que define los métodos del repositorio de productos
    public interface IProductRepository
    {
        // Método para obtener todos los productos de forma asíncrona
        Task<IEnumerable<Product>> GetAllAsync();

        // Método para obtener un producto específico por su ID de forma asíncrona
        Task<Product> GetByIdAsync(Guid id);

        // Método para agregar un nuevo producto de forma asíncrona
        Task AddAsync(Product product);

        // Método para actualizar un producto existente de forma asíncrona
        Task UpdateAsync(Product product);

        // Método para eliminar un producto por su ID de forma asíncrona
        Task DeleteAsync(Guid id);
    }

}
