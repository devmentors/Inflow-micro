using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Inflow.Services.Customers.Core.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Services.Customers.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IDispatcher _dispatcher;

        public NotificationsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        
        [HttpPost("subscribe")]
        [SwaggerOperation("Subscribe notification")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(SubscribeNotification command)
        {
            await _dispatcher.SendAsync(command);
            return NoContent();
        }
        
        [HttpPost("process")]
        [SwaggerOperation("Process notification")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(ProcessNotification command)
        {
            await _dispatcher.SendAsync(command);
            return NoContent();
        }
    }
}