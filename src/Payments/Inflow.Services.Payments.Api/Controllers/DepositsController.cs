using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Inflow.Services.Payments.Core.Contexts;
using Inflow.Services.Payments.Core.Deposits.Commands;
using Inflow.Services.Payments.Core.Deposits.DTO;
using Inflow.Services.Payments.Core.Deposits.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Services.Payments.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DepositsController : Controller
{
    private const string Policy = "deposits";
    private readonly IDispatcher _dispatcher;
    private readonly IContext _context;

    public DepositsController(IDispatcher dispatcher, IContext context)
    {
        _dispatcher = dispatcher;
        _context = context;
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation("Browse deposits")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<PagedResult<DepositDto>>> BrowseAsync([FromQuery] BrowseDeposits query)
    {
        if (query.CustomerId.HasValue || _context.Identity.IsUser())
        {
            // Customer cannot access the other deposits
            query.CustomerId = _context.Identity.IsUser() ? _context.Identity.Id : query.CustomerId;
        }

        return Ok(await _dispatcher.QueryAsync(query));
    }

    [HttpPost]
    [Authorize]
    [SwaggerOperation("Start deposit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Post(StartDeposit command)
    {
        await _dispatcher.SendAsync(command.Bind(x => x.CustomerId, _context.Identity.Id));
        return NoContent();
    }

    // Acting as a webhook for 3rd party payments service
    [HttpPut("{depositId:guid}/complete")]
    [SwaggerOperation("Complete deposit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Post(Guid depositId, CompleteDeposit command)
    {
        await _dispatcher.SendAsync(command.Bind(x => x.DepositId, depositId));
        return NoContent();
    }
}