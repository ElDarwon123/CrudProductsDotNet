using CrudProducts.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudProducts.Application.Queries
{
    // Clase de consulta para obtener un producto por su identificador único (ID)
    public class GetProductByIdQuery : IRequest<Product>
    {
        // Propiedad que almacena el ID del producto a buscar
        public Guid id { get; set; }
    }
}
