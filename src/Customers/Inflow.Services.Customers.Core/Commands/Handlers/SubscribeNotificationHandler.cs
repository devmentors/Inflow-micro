using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Inflow.Services.Customers.Core.Clients;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Commands.Handlers
{
    internal sealed class SubscribeNotificationHandler : ICommandHandler<SubscribeNotification>
    {
        private readonly IUserApiClient _userApiClient;
        private readonly ILogger<SubscribeNotificationHandler> _logger;

        public SubscribeNotificationHandler(IUserApiClient userApiClient, ILogger<SubscribeNotificationHandler> logger)
        {
            _userApiClient = userApiClient;
            _logger = logger;
        }
        
        public async Task HandleAsync(SubscribeNotification command)
        {
            await _userApiClient.SubscribeNotificationAsync(command.Name, command.CallbackUrl);
        }
    }
}