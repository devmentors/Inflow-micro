using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Inflow.Services.Payments.Core.Contexts;
using Inflow.Services.Payments.Core.Withdrawals.Commands;
using Inflow.Services.Payments.Core.Withdrawals.DTO;
using Inflow.Services.Payments.Core.Withdrawals.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Services.Payments.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WithdrawalsController : Controller
    {
        private const string Policy = "withdrawals";
        private readonly IDispatcher _dispatcher;
        private readonly IContext _context;

        public WithdrawalsController(IDispatcher dispatcher, IContext context)
        {
            _dispatcher = dispatcher;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        [SwaggerOperation("Browse withdrawals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResult<WithdrawalDto>>> BrowseAsync([FromQuery] BrowseWithdrawals query)
        {
            if (query.CustomerId.HasValue || _context.Identity.IsUser())
            {
                // Customer cannot access the other withdrawals
                query.CustomerId = _context.Identity.IsUser() ? _context.Identity.Id : query.CustomerId;
            }
            
            return Ok(await _dispatcher.QueryAsync(query));
        }
        
        [HttpPost]
        [Authorize]
        [SwaggerOperation("Start withdrawal")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Post(StartWithdrawal command)
        {
            await _dispatcher.SendAsync(command.Bind(x => x.CustomerId, _context.Identity.Id));
            return NoContent();
        }
    }
}
