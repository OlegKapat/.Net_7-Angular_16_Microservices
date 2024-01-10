using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Aplication.Exeptions
{
    public class OrderNotFoundException : ApplicationException
    {
        public OrderNotFoundException(string name, object key) : base($"Entity {name} - {key} is not found.")
        {
        }
    }
}