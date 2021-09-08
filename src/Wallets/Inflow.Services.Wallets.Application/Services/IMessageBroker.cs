using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Inflow.Services.Wallets.Application.Services
{
    internal interface IMessageBroker
    {
        Task PublishAsync(params IEvent[] events);
    }
}