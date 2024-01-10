using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Discount.Application.Commands
{
    public class DeleteDiscountCommand:IRequest<bool>
    {
        public string ProductName { get; set; }
        public DeleteDiscountCommand(string productName)
        {
            ProductName = productName;
            
        }
    }
}