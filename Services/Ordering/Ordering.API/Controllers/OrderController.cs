using System.Net;
using Common.Logging.Correlation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Aplication.Commands;
using Ordering.Aplication.Queries;
using Ordering.Aplication.Responses;

namespace Ordering.API.Controllers
{
    public class OrderController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderController> _logger;
        private readonly ICorrelationIdGenerator _correlationId;
       

        public OrderController(IMediator mediator, ILogger<OrderController> logger, ICorrelationIdGenerator correlationId)
        {
            _correlationId = correlationId;
            _logger = logger;
            _mediator = mediator;
            _logger.LogInformation("CorrelationId {correlationId}:", _correlationId.Get());
        }
      
        [HttpGet("GetOrdersByUserName/{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<OrderResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(
            string userName
        )
        {
            var query = new GetOrderListQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

        [HttpPost("CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CkeckOutOrder([FromBody] CheckoutOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            var result = await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("DeleteOrder/{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var cmd = new DeleteOrderCommand() { Id = id };
            await _mediator.Send(cmd);
            return NoContent();
        }
    }
}
