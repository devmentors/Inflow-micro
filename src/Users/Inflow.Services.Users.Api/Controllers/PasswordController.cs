using System.Threading.Tasks;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Inflow.Services.Users.Core.Commands;
using Inflow.Services.Users.Core.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Services.Users.Api.Controllers;

public class PasswordController : BaseController
{
    private readonly IDispatcher _dispatcher;
    private readonly IContext _context;

    public PasswordController(IDispatcher dispatcher, IContext context)
    {
        _dispatcher = dispatcher;
        _context = context;
    }
        
    [Authorize]
    [HttpPut]
    [SwaggerOperation("Change password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> ChangeAsync(ChangePassword command)
    {
        await _dispatcher.SendAsync(command.Bind(x => x.UserId, _context.Identity.Id));
        return NoContent();
    }
}