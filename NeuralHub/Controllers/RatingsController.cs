using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hubs.Controllers
{
    [ApiController]
    [Route("api/[ratings]")]
    public class RatingsController : ControllerBase
    {
        private readonly ISender _mediator;
        public RatingsController(ISender mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Post(SubmitRatingCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(null, new { id }, null);
        }
    }
}
