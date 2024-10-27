using MediatR;
using Microsoft.AspNetCore.Mvc;
using CrudProducts.Application.Commands;
using CrudProducts.Application.Queries;
using CrudProducts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;

namespace CrudProducts.API.Controllers
{
    // Se define la ruta base para el controlador
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Constructor que inyecta el mediador para manejar comandos y consultas
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //    GET METHODS
        //  GET: api/products
        /// <summary>
        /// Obtener todos los productos.
        /// </summary>
        /// <returns>Lista de productos.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        [EndpointSummary("Listar productos")]
        [EndpointDescription("Poder listar todos los productos existentes en la base de datos")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);

            // Si no se encuentran productos, lanza una excepción
            if (products == null || !products.Any())
            {
                return NotFound("No hay productos");
            }

            return Ok(products); // Devuelve los productos encontrados
        }

        /// <summary>
        /// Obtener un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto.</param>
        /// <returns>Producto correspondiente al ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Obtener un producto")]
        [EndpointDescription("EndPoint para obtener un solo producto por su id")]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            var product = await _mediator.Send(new GetProductByIdQuery { id = id });
            if (product == null)
            {
                return NotFound(new { message = $"El producto con id: {id} no fue encontrado" });
            }
            return Ok(product); // Devuelve el producto encontrado
        }

        //      POST METHOD

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="command">Comando para crear el producto.</param>
        /// <param name="imageFile">Archivo de imagen para el producto.</param>
        /// <returns>Producto creado.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType, Type = typeof(string))]
        [EndpointSummary("Crear un producto")]
        [EndpointDescription("EndPoint para poder crear un producto, tener en cuenta que se debe hacer con multipart/form-data si se desea realizar una petición con un cliente")]
        [EndpointName("Crear")]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] CreateProductCommand command, IFormFile? imageFile)
        {
            // Verifica si el modelo es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica que el producto no sea nulo
            if (command == null)
            {
                return BadRequest(nameof(command) + "El producto no puede ser nulo.");
            }
            // Verifica que el nombre del producto no esté vacío
            if (string.IsNullOrWhiteSpace(command.Nombre))
            {
                return BadRequest("El nombre del producto es obligatorio.");
            }
            // Verifica que el precio del producto sea mayor que cero
            if (command.Precio <= 0)
            {
                return BadRequest("El precio debe ser mayor que cero.");
            }

            const int maxSizeInBytes = 5 * 1024 * 1024; // Tamaño máximo de la imagen (5 MB)
            if (imageFile != null && imageFile.Length > maxSizeInBytes)
            {
                return BadRequest("La imagen no debe exceder los 5 MB.");
            }

            // Procesa la imagen si se proporciona
            if (imageFile != null)
            {
                try
                {
                    using var memoryStream = new MemoryStream();
                    await imageFile.CopyToAsync(memoryStream);
                    memoryStream.Position = 0; // Reinicia la posición del flujo de memoria

                    // Carga y redimensiona la imagen
                    using (var image = Image.Load(memoryStream.ToArray()))
                    {
                        image.Mutate(x => x.Resize(800, 600)); // Redimensiona la imagen a 800x600

                        using var compressedStream = new MemoryStream();
                        await image.SaveAsJpegAsync(compressedStream); // Guarda la imagen en formato JPEG

                        command.Imagen = compressedStream.ToArray(); // Asigna la imagen comprimida al comando
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Error al procesar la imagen: " + ex.Message);
                }
            }

            var product = await _mediator.Send(command); // Envía el comando para crear el producto
            return CreatedAtAction(nameof(GetProductById), new { id = product.id }, product); // Devuelve el producto creado
        }

        //      PATCH METHOD

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        /// <param name="id">ID del producto a actualizar.</param>
        /// <param name="command">Comando con los datos de actualización.</param>
        /// <returns>Producto actualizado.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Actualizar un producto")]
        [EndpointDescription("EndPoint para poder actualizar un solo producto por su id")]
        public async Task<ActionResult> UpdateProduct(Guid id, [FromBody] PatchProductCommand command)
        {
            // Verifica si el modelo es válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _mediator.Send(new GetProductByIdQuery { id = id });
            if (product == null)
            {
                return NotFound(new { message = "El producto que intentas actualizar no existe" });
            }

            // Verifica que el producto no sea nulo
            if (command == null)
            {
                return BadRequest("El producto no puede ser nulo.");
            }
            // Verifica que el precio del producto sea mayor que cero
            if (command.Precio <= 0)
            {
                return BadRequest("El precio debe ser mayor que cero.");
            }

            command.id = id; // Asigna el ID al comando

            var updatedProduct = await _mediator.Send(command); // Envía el comando para actualizar el producto

            return Ok(updatedProduct); // Devuelve el producto actualizado
        }

        //      DELETE METHOD
        /// <summary>
        /// Elimina un producto por su ID.
        /// </summary>
        /// <param name="id">ID del producto a eliminar.</param>
        /// <returns>NoContent si se elimina correctamente.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [EndpointSummary("Eliminar un producto")]
        [EndpointDescription("EndPoint para poder eliminar un producto por su id")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {

            var product = await _mediator.Send(new GetProductByIdQuery { id = id });
            // Si no se encuentra el producto, lanza una excepción
            if (product == null)
            {
                return NotFound("No se pudo encontrar el producto que se quiere eliminar.");
            }

            var command = new DeleteProductCommand(id); // Crea el comando para eliminar el producto

            await _mediator.Send(command); // Envía el comando para eliminar el producto

            return NoContent(); // Devuelve 204 No Content si se elimina correctamente
        }
    }
}
