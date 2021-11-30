using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Inflow.Services.Users.Core.Services;

internal interface IMessageBroker
{
    Task PublishAsync(params IEvent[] events);
}