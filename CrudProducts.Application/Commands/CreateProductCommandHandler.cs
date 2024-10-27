using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CrudProducts.Application.Interfaces;
using CrudProducts.Domain.Entities;


namespace CrudProducts.Application.Commands
{
    // Clase que maneja la lógica para procesar el comando CreateProductCommand y crear un nuevo producto
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        // Repositorio de productos, usado para acceder y almacenar productos en la base de datos
        private readonly IProductRepository _productRepository;

        // Inyección del repositorio de productos
        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Método que maneja el comando CreateProductCommand y crea un nuevo producto
        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Crea una instancia de Product con los valores del comando de creación
            var product = new Product
            {
                id = Guid.NewGuid(), // Genera un identificador único para cada producto
                Nombre = request.Nombre, // Nombre del producto desde el comando
                Descripcion = request.Descripcion, // Descripción del producto, si está presente
                Imagen = request.Imagen, // Imagen del producto en bytes, si está presente
                Precio = request.Precio, // Precio del producto
                Descuento = request.Descuento // Descuento del producto, si está presente
            };

            // Añade el producto al repositorio de productos y guarda en la base de datos
            await _productRepository.AddAsync(product);

            // Retorna el producto recién creado
            return product;
        }
    }

}
