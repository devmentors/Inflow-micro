using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Inflow.Services.Users.Core.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Services.Users.Api.Controllers
{
    public class NotificationsController : BaseController
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
    }
}