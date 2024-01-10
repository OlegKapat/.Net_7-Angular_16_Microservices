using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Aplication.Commands;
using Ordering.Aplication.Exeptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Aplication.Handlers
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        public ILogger<DeleteOrderCommandHandler> _logger;

        public DeleteOrderCommandHandler(
            IOrderRepository orderRepository,
            ILogger<DeleteOrderCommandHandler> logger
        )
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<bool> Handle(
            DeleteOrderCommand request,
            CancellationToken cancellationToken
        )
        {
            var orderToDelete = await _orderRepository.GetByIdAsync(request.Id);
            if (orderToDelete == null)
            {
                throw new OrderNotFoundException(nameof(Order), request.Id);
            }
            await _orderRepository.DeleteAsync(orderToDelete);
            _logger.LogInformation($"Order with Id {request.Id} is deleted successfully.");
            return Convert.ToInt32(orderToDelete.State) > 2;
        }
    }
}
