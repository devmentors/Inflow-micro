using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.Auth;
using Convey.CQRS.Commands;
using Inflow.Services.Users.Core.DTO;
using Inflow.Services.Users.Core.Entities;
using Inflow.Services.Users.Core.Events;
using Inflow.Services.Users.Core.Exceptions;
using Inflow.Services.Users.Core.Repositories;
using Inflow.Services.Users.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Users.Core.Commands.Handlers
{
    internal sealed class SignInHandler : ICommandHandler<SignIn>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRequestStorage _userRequestStorage;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<SignInHandler> _logger;

        public SignInHandler(IUserRepository userRepository, IJwtHandler jwtHandler,
            IPasswordHasher<User> passwordHasher, IUserRequestStorage userRequestStorage, IMessageBroker messageBroker,
            ILogger<SignInHandler> logger)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
            _passwordHasher = passwordHasher;
            _userRequestStorage = userRequestStorage;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task HandleAsync(SignIn command)
        {
            var user = await _userRepository.GetAsync(command.Email.ToLowerInvariant());
            if (user is null)
            {
                throw new InvalidCredentialsException();
            }

            if (user.State != UserState.Active)
            {
                throw new UserNotActiveException(user.Id);
            }

            if (_passwordHasher.VerifyHashedPassword(default, user.Password, command.Password) ==
                PasswordVerificationResult.Failed)
            {
                throw new InvalidCredentialsException();
            }

            var claims = new Dictionary<string, IEnumerable<string>>
            {
                ["permissions"] = user.Role.Permissions
            };

            var jwt = _jwtHandler.CreateToken(user.Id.ToString(), user.Role.Name, claims: claims);
            var dto = new JwtDto
            {
                AccessToken = jwt.AccessToken,
                Claims = jwt.Claims,
                Expiry = jwt.Expires,
                Role = jwt.Role,
                UserId = user.Id,
                Email = user.Email
            };
            await _messageBroker.PublishAsync(new SignedIn(user.Id));
            _logger.LogInformation($"User with ID: '{user.Id}' has signed in.");
            _userRequestStorage.SetToken(command.Id, dto);
        }
    }
}