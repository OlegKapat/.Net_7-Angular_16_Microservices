using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Aplication.Commands;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        public IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumer> _logger;

        public BasketOrderingConsumer(
            IMediator mediator,
            IMapper mapper,
            ILogger<BasketOrderingConsumer> logger
        )
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
           var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
           var result = await _mediator.Send(command);
             _logger.LogInformation($"Basket checkout event completed!!!");
        }
    }
}
