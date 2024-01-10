using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Ordering.Aplication.Responses;

namespace Ordering.Aplication.Queries
{
    public class GetOrderListQuery:IRequest<List<OrderResponse>>
    {
        public string UserName { get; set; }
        public GetOrderListQuery(string userName)
        {
            UserName = userName;
        }
    }
}