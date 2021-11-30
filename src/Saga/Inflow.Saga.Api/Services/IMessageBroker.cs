using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Convey.CQRS.Events;

namespace Inflow.Saga.Api.Services;

internal interface IMessageBroker
{
    Task PublishAsync(params IEvent[] events);
    Task SendAsync(params ICommand[] commands);
}