using CQRSQuery.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRSQuery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetOrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _mediator.Send(new GetOrdersQuery());

            if (orders != null && orders.Any())
                return Ok(orders);
            else
                return NotFound("No orders found.");
        }
    }
}
