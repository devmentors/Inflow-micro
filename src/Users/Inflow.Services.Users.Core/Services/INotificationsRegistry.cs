using System.Threading.Tasks;
using Convey.CQRS.Events;

namespace Inflow.Services.Users.Core.Services
{
    public interface INotificationsRegistry
    {
        Task RegisterAsync(string name, string callbackUrl);
        Task NotifyAsync<T>(T @event) where T : IEvent;
    }
}