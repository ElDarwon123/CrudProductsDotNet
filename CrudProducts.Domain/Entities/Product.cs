using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudProducts.Domain.Entities
{
    // Clase que representa un producto en el sistema
    public class Product
    {
        // Propiedad que representa el identificador único del producto
        [Key] // Indica que esta propiedad es la clave primaria
        public Guid id { get; set; }

        // Propiedad que almacena el nombre del producto
        [Required] // Indica que este campo es obligatorio
        public string Nombre { get; set; }

        // Propiedad que almacena una descripción opcional del producto
        public string? Descripcion { get; set; }

        // Propiedad que almacena la imagen del producto en formato de bytes
        public byte[]? Imagen { get; set; }

        // Propiedad que almacena el precio del producto
        [Required] // Indica que este campo es obligatorio
        public decimal Precio { get; set; }

        // Propiedad que almacena un descuento opcional del producto
        public decimal? Descuento { get; set; }
    }
}
