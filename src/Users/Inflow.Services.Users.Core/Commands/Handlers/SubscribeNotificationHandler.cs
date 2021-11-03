using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Inflow.Services.Users.Core.Services;

namespace Inflow.Services.Users.Core.Commands.Handlers
{
    internal sealed class SubscribeNotificationHandler : ICommandHandler<SubscribeNotification>
    {
        private readonly INotificationsRegistry _registry;

        public SubscribeNotificationHandler(INotificationsRegistry registry)
        {
            _registry = registry;
        }
        
        public async Task HandleAsync(SubscribeNotification command)
        {
            await _registry.RegisterAsync(command.Name, command.CallbackUrl);
        }
    }
}