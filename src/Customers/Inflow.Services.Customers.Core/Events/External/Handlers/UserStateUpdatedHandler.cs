using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Inflow.Services.Customers.Core.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Events.External.Handlers;

internal sealed class UserStateUpdatedHandler : IEventHandler<UserStateUpdated>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<UserStateUpdatedHandler> _logger;

    public UserStateUpdatedHandler(ICustomerRepository customerRepository, ILogger<UserStateUpdatedHandler> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }

    public async Task HandleAsync(UserStateUpdated @event, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetAsync(@event.UserId);
        if (customer is null)
        {
            return;
        }

        switch (@event.State.ToLowerInvariant())
        {
            case "active":
                customer.Unlock();
                break;
            case "locked":
                customer.Lock();
                break;
            default:
                _logger.LogWarning($"Received an unknown user state: '{@event.State}'.");
                return;
        }

        await _customerRepository.UpdateAsync(customer);
        _logger.LogInformation($"{(customer.IsActive ? "Unlocked" : "Locked")} " +
                               $"customer with ID: '{customer.Id}'.");
    }
}