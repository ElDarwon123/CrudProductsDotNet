using CrudProducts.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudProducts.Application.Queries
{
    // Clase de consulta para obtener todos los productos
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
        /*
         * Clase vacía que representa una solicitud para obtener una lista de productos.
         Implementa IRequest con un tipo de respuesta de IEnumerable<Product>, indicando
         que el resultado esperado es una colección de objetos Product.
        */
    }
}
