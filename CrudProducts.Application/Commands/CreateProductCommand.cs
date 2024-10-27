using CrudProducts.Application.Interfaces;
using CrudProducts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace CrudProducts.Application.Commands
{
    // Clase que representa el comando para crear un producto
    public class CreateProductCommand : IRequest<Product>
    {
        // Propiedad Nombre del producto; es requerida
        [Required(ErrorMessage = "El nombre del producto es requerido")]
        public string Nombre { get; set; }

        // Propiedad Descripción del producto; es opcional
        public string? Descripcion { get; set; }

        // Propiedad Imagen del producto como un array de bytes; es opcional
        public byte[]? Imagen { get; set; }

        // Propiedad Precio del producto; es requerida
        [Required(ErrorMessage = "El precio del producto es requerido")]
        public decimal Precio { get; set; }

        // Propiedad Descuento del producto; es opcional
        public decimal? Descuento { get; set; }

        // Constructor por defecto
        public CreateProductCommand() { }
    }

}
