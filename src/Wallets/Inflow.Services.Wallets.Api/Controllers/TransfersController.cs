using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Inflow.Services.Wallets.Application.Contexts;
using Inflow.Services.Wallets.Application.Wallets.Commands;
using Inflow.Services.Wallets.Application.Wallets.DTO;
using Inflow.Services.Wallets.Application.Wallets.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Services.Wallets.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TransfersController : Controller
    {
        private const string Policy = "transfers";
        private readonly IDispatcher _dispatcher;
        private readonly IContext _context;

        public TransfersController(IDispatcher dispatcher, IContext context)
        {
            _dispatcher = dispatcher;
            _context = context;
        }
        
        [HttpGet]
        [Authorize(Policy)]
        [SwaggerOperation("Browse transfers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PagedResult<TransferDto>>> BrowseAsync([FromQuery] BrowseTransfers query)
            => Ok(await _dispatcher.QueryAsync(query));

        [HttpGet("{transferId:guid}")]
        [Authorize(Policy)]
        [SwaggerOperation("Get transfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<TransferDetailsDto>> GetAsync(Guid transferId)
        {
            var transfer = await _dispatcher.QueryAsync(new GetTransfer { TransferId = transferId });
            if (transfer is not null)
            {
                return Ok(transfer);
            }

            return NotFound();
        }
        
        [HttpPost("funds")]
        [SwaggerOperation("Transfer funds")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Post(TransferFunds command)
        {
            await _dispatcher.SendAsync(command.Bind(x => x.OwnerId, _context.Identity.Id));
            return NoContent();
        }
        
        [Authorize(Policy)]
        [HttpPost("incoming")]
        [SwaggerOperation("Add funds")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Post(AddFunds command)
        {
            await _dispatcher.SendAsync(command);
            return NoContent();
        }
        
        [Authorize(Policy)]
        [HttpPost("outgoing")]
        [SwaggerOperation("Deduct funds")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Post(DeductFunds command)
        {
            await _dispatcher.SendAsync(command);
            return NoContent();
        }
    }
}