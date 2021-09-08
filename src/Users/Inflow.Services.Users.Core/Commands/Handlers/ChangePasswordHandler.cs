using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Inflow.Services.Users.Core.Entities;
using Inflow.Services.Users.Core.Exceptions;
using Inflow.Services.Users.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Users.Core.Commands.Handlers
{
    internal sealed class ChangePasswordHandler : ICommandHandler<ChangePassword>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILogger<ChangePasswordHandler> _logger;

        public ChangePasswordHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher,
            ILogger<ChangePasswordHandler> logger)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task HandleAsync(ChangePassword command)
        {
            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }
            
            if (_passwordHasher.VerifyHashedPassword(default, user.Password, command.CurrentPassword) ==
                PasswordVerificationResult.Failed)
            {
                throw new InvalidPasswordException("current password is invalid");
            }

            user.Password = _passwordHasher.HashPassword(default, command.NewPassword);;
            await _userRepository.UpdateAsync(user);
            _logger.LogInformation($"User with ID: '{user.Id}' has changed password.");
        }
    }
}