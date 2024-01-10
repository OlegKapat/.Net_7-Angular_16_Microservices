using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Ordering.Aplication.Commands
{
    public class DeleteOrderCommand : IRequest<bool>
    {
         public int Id { get; set; }

    }
}
