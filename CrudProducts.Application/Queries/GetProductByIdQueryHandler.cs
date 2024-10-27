using CrudProducts.Application.Interfaces;
using CrudProducts.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudProducts.Application.Queries
{
    // Clase que maneja la consulta para obtener un producto por su ID
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        // Repositorio para acceder a los datos de productos
        private readonly IProductRepository _productRepository;

        // Constructor que recibe una instancia de IProductRepository
        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository; // Inicializa el repositorio
        }

        // Método que maneja la consulta y devuelve el producto correspondiente al ID
        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // Llama al repositorio para obtener el producto por su ID
            return await _productRepository.GetByIdAsync(request.id);
        }
    }
}
