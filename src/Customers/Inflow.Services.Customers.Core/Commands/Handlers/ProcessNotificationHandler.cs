using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.WebApi.CQRS;
using Inflow.Services.Customers.Core.Events.External;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Commands.Handlers
{
    internal sealed class ProcessNotificationHandler : ICommandHandler<ProcessNotification>
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new JsonStringEnumConverter()}
        };
        
        private readonly IDispatcher _dispatcher;
        private readonly ILogger<ProcessNotificationHandler> _logger;

        public ProcessNotificationHandler(IDispatcher dispatcher, ILogger<ProcessNotificationHandler> logger)
        {
            _dispatcher = dispatcher;
            _logger = logger;
        }

        public async Task HandleAsync(ProcessNotification command)
        {
            _logger.LogInformation($"Received a notification: '{command.Name}'.");
            if (command.Name == "setup")
            {
                return;
            }

            if (command.Name == "signed_up")
            {
                var payload = Map<SignedUp>(command.Data);
                await _dispatcher.PublishAsync(payload);
            }
        }
        
        private static T Map<T>(object data)
            => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(data, SerializerOptions), SerializerOptions);
    }
}