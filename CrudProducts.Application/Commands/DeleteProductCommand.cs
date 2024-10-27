using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrudProducts.Application.Commands
{
    // Comando para la eliminación de un producto
    public class DeleteProductCommand : IRequest<Unit>
    {
        // Identificador único del producto que se desea eliminar
        public Guid Id { get; set; }

        // Constructor que inicializa el comando con el ID del producto a eliminar
        public DeleteProductCommand(Guid id)
        {
            Id = id;
        }
    }

}
