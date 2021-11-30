using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Inflow.Services.Wallets.Core.Shared.Types;

internal abstract class AggregateRoot<T>
{
    public T Id { get; protected set; }
    public int Version { get; protected set; } = 1;
    public IEnumerable<IDomainEvent> Events => _events;
        
    private readonly List<IDomainEvent> _events = new();
    private bool _versionIncremented;

    protected void AddEvent(IDomainEvent @event, CancellationToken cancellationToken = default)
    {
        if (!_events.Any() && !_versionIncremented)
        {
            Version++;
            _versionIncremented = true;
        }
            
        _events.Add(@event);
    }

    public void ClearEvents() => _events.Clear();

    protected void IncrementVersion()
    {
        if (_versionIncremented)
        {
            return;
        }
            
        Version++;
        _versionIncremented = true;
    }
}