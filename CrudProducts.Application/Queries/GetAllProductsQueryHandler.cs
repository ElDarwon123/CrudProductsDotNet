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
    // Clase manejadora de la consulta para obtener todos los productos
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        // Campo de solo lectura para el repositorio de productos
        private readonly IProductRepository _productRepository;

        // Constructor que inyecta el repositorio de productos
        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Método manejador de la consulta que se ejecuta cuando se solicita la lista de todos los productos
        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            // Llama al método del repositorio para obtener todos los productos y los retorna
            return await _productRepository.GetAllAsync();
        }
    }
}
