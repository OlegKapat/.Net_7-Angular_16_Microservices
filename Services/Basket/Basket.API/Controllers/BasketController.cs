using Basket.Application.Commands;
using Basket.Application.GrpcService;
using Basket.Application.Mappers;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Common.Logging.Correlation;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    public class BasketController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BasketController> _logger;
        public readonly IPublishEndpoint _publishEndpoint;
        public readonly ICorrelationIdGenerator _correlationId;

        public BasketController(
            IMediator mediator,
            IPublishEndpoint publishEndpoint,
            ICorrelationIdGenerator correlationId,
            ILogger<BasketController> logger
        )
        {
            _publishEndpoint = publishEndpoint;
            _mediator = mediator;
            _correlationId = correlationId;
            _logger = logger;
            _logger.LogInformation("CorrelationId {correlationId}:", _correlationId.Get());
        }

        [HttpGet]
        [Route("[action]/{userName}", Name = "GetBasketByUserName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCartResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost]
        [Route("CreateBasket")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ShoppingCartResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket(
            [FromBody] CreateShoppingCartCommand createShoppingCartCommand
        )
        {
            var basket = await _mediator.Send(createShoppingCartCommand);
            return Ok(basket);
        }

        [HttpDelete]
        [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
        {
            var query = new DeleteBasketByUserNameQuery(userName);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CheckOut([FromBody] BasketCheckout basketCheckout)
        {
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);
            if (basket == null)
            {
                return BadRequest();
            }
            var eventMesg = BasketMapper.Mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMesg.TotalPrice = basket.TotalPrice;
            eventMesg.CorrelationId = _correlationId.Get();
            await _publishEndpoint.Publish(eventMesg);
            var deleteQuery = new DeleteBasketByUserNameQuery(basketCheckout.UserName);
            await _mediator.Send(deleteQuery);
            return Accepted();
        }
    }
}
