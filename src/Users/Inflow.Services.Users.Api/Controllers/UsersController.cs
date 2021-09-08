using System;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Convey.WebApi;
using Convey.WebApi.CQRS;
using Inflow.Services.Users.Core.Commands;
using Inflow.Services.Users.Core.DTO;
using Inflow.Services.Users.Core.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Inflow.Services.Users.Api.Controllers
{
    [Authorize(Policy)]
    public class UsersController : BaseController
    {
        private readonly IDispatcher _dispatcher;
        private const string Policy = "users";

        public UsersController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet("{userId:guid}")]
        [SwaggerOperation("Get user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<UserDetailsDto>> GetAsync(Guid userId)
            => OkOrNotFound(await _dispatcher.QueryAsync(new GetUser {UserId = userId}));

        // On purpose, for sync communication sample
        [HttpGet("by-email/{email}")]
        [AllowAnonymous]
        [SwaggerOperation("Get user by email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDetailsDto>> GetByEmailAsync(string email)
            => OkOrNotFound(await _dispatcher.QueryAsync(new GetUserByEmail { Email = email }));
        
        [HttpGet]
        [SwaggerOperation("Browse users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PagedResult<UserDto>>> BrowseAsync([FromQuery] BrowseUsers query)
            => Ok(await _dispatcher.QueryAsync(query));

        [HttpPut("{userId:guid}/state")]
        [SwaggerOperation("Update user state")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UpdateStateAsync(Guid userId, UpdateUserState command)
        {
            await _dispatcher.SendAsync(command.Bind(x => x.UserId, userId));
            return NoContent();
        }
    }
}