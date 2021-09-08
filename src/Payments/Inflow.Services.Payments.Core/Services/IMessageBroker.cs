using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Inflow.Services.Payments.Core.Services
{
    internal interface IMessageBroker
    {
        Task PublishAsync(params IEvent[] events);
    }
}