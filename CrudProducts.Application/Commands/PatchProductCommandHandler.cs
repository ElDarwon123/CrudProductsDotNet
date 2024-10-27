using CrudProducts.Application.Interfaces;
using CrudProducts.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudProducts.Application.Commands
{
    // Manejador para el comando PatchProductCommand, el cual actualiza parcialmente un producto
    public class PatchProductCommandHandler : IRequestHandler<PatchProductCommand, Product>
    {
        // Repositorio de productos para interactuar con la base de datos
        private readonly IProductRepository _productRepository;

        // Constructor que inyecta el repositorio de productos
        public PatchProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Método que maneja el comando para actualizar el producto
        public async Task<Product> Handle(PatchProductCommand request, CancellationToken cancellationToken)
        {
            // Busca el producto en el repositorio por su ID
            var product = await _productRepository.GetByIdAsync(request.id);

            // Si el producto no se encuentra, lanza una excepción
            if (product == null)
            {
                throw new KeyNotFoundException("Producto no encontrado");
            }

            // Actualiza el nombre del producto solo si se ha proporcionado uno nuevo
            if (!string.IsNullOrWhiteSpace(request.Nombre))
            {
                product.Nombre = request.Nombre;
            }

            // Actualiza el producto en el repositorio
            await _productRepository.UpdateAsync(product);

            // Devuelve el producto actualizado
            return product;
        }
    }

}
