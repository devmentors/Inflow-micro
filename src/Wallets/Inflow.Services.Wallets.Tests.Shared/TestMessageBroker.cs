using System.Collections.Generic;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Inflow.Services.Wallets.Application.Services;

namespace Inflow.Services.Wallets.Tests.Shared;

public class TestMessageBroker : IMessageBroker
{
    private readonly List<IEvent> _events = new();

    public IReadOnlyList<IEvent> Events => _events;
        
    public Task PublishAsync(params IEvent[] events)
    {
        _events.AddRange(@events);
        return Task.CompletedTask;
    }
}