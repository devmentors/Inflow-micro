using System.Threading.Tasks;
using Convey.WebApi.CQRS;
using Inflow.Services.Users.Core.Commands;
using Inflow.Services.Users.Core.Contexts;
using Inflow.Services.Users.Core.DTO;
using Inflow.Services.Users.Core.Queries;
using Inflow.Services.Users.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Services.Users.Api.Controllers;

public class AccountController : BaseController
{
    private readonly IDispatcher _dispatcher;
    private readonly IContext _context;
    private readonly IUserRequestStorage _userRequestStorage;

    public AccountController(IDispatcher dispatcher, IContext context, IUserRequestStorage userRequestStorage)
    {
        _dispatcher = dispatcher;
        _context = context;
        _userRequestStorage = userRequestStorage;
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation("Get account")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDetailsDto>> GetAsync()
        => OkOrNotFound(await _dispatcher.QueryAsync(new GetUser {UserId = _context.Identity.Id}));

    [HttpPost("sign-up")]
    [SwaggerOperation("Sign up")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> SignUpAsync(SignUp command)
    {
        await _dispatcher.SendAsync(command);
        return NoContent();
    }

    [HttpPost("sign-in")]
    [SwaggerOperation("Sign in")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<JwtDto>> SignInAsync(SignIn command)
    {
        await _dispatcher.SendAsync(command);
        var jwt = _userRequestStorage.GetToken(command.Id);
        return Ok(jwt);
    }

    [Authorize]
    [HttpDelete("sign-out")]
    [SwaggerOperation("Sign out")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> SignOutAsync()
    {
        await _dispatcher.SendAsync(new SignOut(_context.Identity.Id));
        return NoContent();
    }
}