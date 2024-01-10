using AutoMapper;
using MediatR;
using Ordering.Aplication.Queries;
using Ordering.Aplication.Responses;
using Ordering.Core.Repositories;

namespace Ordering.Aplication.Handlers
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public GetOrderListQueryHandler(IOrderRepository orderRepository,IMapper mapper)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderResponse>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
            return _mapper.Map<List<OrderResponse>>(orderList);
        }
    }
}