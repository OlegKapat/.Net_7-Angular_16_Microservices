using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Aplication.Commands;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMediator _mediator;
        public IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumerV2> _logger;

        public BasketOrderingConsumerV2(
            IMediator mediator,
            IMapper mapper,
            ILogger<BasketOrderingConsumerV2> logger
        )
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            using var scope = _logger.BeginScope(
                "Consuming Basket Checkout Event for {correlationId}",
                context.Message.CorrelationId
            );
            var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
            var result = await _mediator.Send(command);
            _logger.LogInformation($"Basket checkout event completed!!!");
        }
        private static void PopulateAddressDetails(CheckoutOrderCommand command)
    {
        command.FirstName = "Oleh";
        command.LastName = "Bashuk";
        command.EmailAddress = "oleh@eshop.net";
        command.AddressLine = "Kyiv";
        command.Country = "Ukraine";
        command.State = "none";
        command.ZipCode = "03000";
        command.PaymentMethod = 1;
        command.CardName = "Visa";
        command.CardNumber = "1234567890123456";
        command.Expiration = "12/25";
        command.CVV = "123";
    }
    }
}
