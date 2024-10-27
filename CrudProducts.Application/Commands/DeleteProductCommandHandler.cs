using CrudProducts.Application.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace CrudProducts.Application.Commands
{
    // Manejador para el comando de eliminación de un producto
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        // Repositorio de productos para acceder a las operaciones de datos
        private readonly IProductRepository _productRepository;

        // Inyección del repositorio de productos
        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Método que maneja la lógica para eliminar un producto
        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // Busca el producto por su ID en el repositorio
            var product = await _productRepository.GetByIdAsync(request.Id);

            // Si el producto no se encuentra, lanza una excepción
            if (product == null)
            {
                throw new KeyNotFoundException("Producto no encontrado");
            }

            // Elimina el producto del repositorio
            await _productRepository.DeleteAsync(request.Id);

            // Devuelve Unit.Value, este representa un valor vacío
            return Unit.Value;
        }
    }

}
