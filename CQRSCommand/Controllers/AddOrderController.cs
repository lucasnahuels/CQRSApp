using CQRSCommand.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRSCommand.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddOrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AddOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderCommand command)
        {
            var result = await _mediator.Send(command);

            if (result)
                return Ok("Order added successfully.");
            else
                return StatusCode(500, "Failed to add order.");
        }
    }
}
