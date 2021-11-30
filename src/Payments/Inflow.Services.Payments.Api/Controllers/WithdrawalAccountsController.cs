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

namespace Inflow.Services.Payments.Api.Controllers;

[ApiController]
[Route("withdrawals/accounts")]
public class WithdrawalAccountsController : Controller
{
    private readonly IDispatcher _dispatcher;
    private readonly IContext _context;

    public WithdrawalAccountsController(IDispatcher dispatcher, IContext context)
    {
        _dispatcher = dispatcher;
        _context = context;
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation("Browse withdrawal accounts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResult<WithdrawalAccountDto>>> BrowseAsync([FromQuery] BrowseWithdrawalAccounts query)
    {
        if (query.CustomerId.HasValue || _context.Identity.IsUser())
        {
            // Customer cannot access the other withdrawal accounts
            query.CustomerId = _context.Identity.IsUser() ? _context.Identity.Id : query.CustomerId;
        }
            
        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpPost]
    [Authorize]
    [SwaggerOperation("Add withdrawal account")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Post(AddWithdrawalAccount command)
    {
        await _dispatcher.SendAsync(command.Bind(x => x.CustomerId, _context.Identity.Id));
        return NoContent();
    }
}