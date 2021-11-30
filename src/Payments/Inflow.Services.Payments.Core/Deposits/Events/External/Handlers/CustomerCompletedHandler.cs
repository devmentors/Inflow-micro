using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Inflow.Services.Payments.Shared.Entities;
using Inflow.Services.Payments.Shared.Repositories;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Payments.Core.Deposits.Events.External.Handlers;

internal sealed class CustomerCompletedHandler : IEventHandler<CustomerCompleted>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CustomerCompletedHandler> _logger;

    public CustomerCompletedHandler(ICustomerRepository customerRepository,
        ILogger<CustomerCompletedHandler> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }
        
    public async Task HandleAsync(CustomerCompleted @event, CancellationToken cancellationToken = default)
    {
        var customer = new Customer(@event.CustomerId, @event.FullName, @event.Nationality);
        await _customerRepository.AddAsync(customer);
    }
}