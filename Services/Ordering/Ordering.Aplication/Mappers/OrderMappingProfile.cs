using AutoMapper;
using Ordering.Aplication.Commands;
using Ordering.Aplication.Responses;
using Ordering.Core.Entities;

namespace Ordering.Aplication.Mappers
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();
        }
    }
}
