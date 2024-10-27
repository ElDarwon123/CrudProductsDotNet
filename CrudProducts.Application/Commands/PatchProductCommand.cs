using CrudProducts.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudProducts.Application.Commands
{
    // Comando para actualizar parcialmente un producto existente
    public class PatchProductCommand : IRequest<Product>
    {
        // Identificador único del producto a actualizar
        public Guid id { get; set; }

        // Propiedades opcionales que pueden actualizarse en el producto
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal? Precio { get; set; }
        public decimal? Descuento { get; set; }

        // Constructor que inicializa las propiedades del comando
        public PatchProductCommand(Guid Id, string? nombre, string? descripcion, decimal? precio, decimal? descuento)
        {
            // Asigna valores a las propiedades para el comando de actualización
            id = Id;
            Nombre = nombre;
            Descripcion = descripcion;
            Precio = precio;
            Descuento = descuento;
        }
    }
}
