using System;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Inflow.Services.Users.Core.Entities;
using Inflow.Services.Users.Core.Events;
using Inflow.Services.Users.Core.Exceptions;
using Inflow.Services.Users.Core.Repositories;
using Inflow.Services.Users.Core.Services;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Users.Core.Commands.Handlers
{
    internal sealed class UpdateUserStateHandler : ICommandHandler<UpdateUserState>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<UpdateUserStateHandler> _logger;

        public UpdateUserStateHandler(IUserRepository userRepository, IMessageBroker messageBroker,
            ILogger<UpdateUserStateHandler> logger)
        {
            _userRepository = userRepository;
            _messageBroker = messageBroker;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateUserState command)
        {
            if (!Enum.TryParse<UserState>(command.State, true, out var state))
            {
                throw new InvalidUserStateException(command.State);
            }

            var user = await _userRepository.GetAsync(command.UserId);
            if (user is null)
            {
                throw new UserNotFoundException(command.UserId);
            }

            var previousState = user.State;
            if (previousState == state)
            {
                return;
            }
            
            if (user.RoleId == Role.Admin)
            {
                throw new UserStateCannotBeChangedException(command.State, command.UserId);
            }

            user.State = state;
            await _userRepository.UpdateAsync(user);
            await _messageBroker.PublishAsync(new UserStateUpdated(user.Id, state.ToString().ToLowerInvariant()));
            _logger.LogInformation($"Updated state for user with ID: '{user.Id}', '{previousState}' -> '{user.State}'.");
        }
    }
}