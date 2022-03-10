using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Events;
using Inflow.Services.Customers.Core.Commands.Handlers;
using Inflow.Services.Customers.Core.Domain.Entities;
using Inflow.Services.Customers.Core.Domain.Repositories;
using Inflow.Services.Customers.Core.Exceptions;
using Inflow.Services.Customers.Core.Services;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Events.External.Handlers;

internal sealed class SignedUpHandler : IEventHandler<SignedUp>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger<CreateCustomerHandler> _logger;

    public SignedUpHandler(ICustomerRepository customerRepository, IClock clock, IMessageBroker messageBroker,
        ILogger<CreateCustomerHandler> logger)
    {
        _customerRepository = customerRepository;
        _clock = clock;
        _messageBroker = messageBroker;
        _logger = logger;
    }
    
    public async Task HandleAsync(SignedUp @event, CancellationToken cancellationToken = default)
    {
        if (@event.Role is not "user")
        {
            return;
        }

        var customerId = @event.UserId;
        if (await _customerRepository.GetAsync(customerId) is not null)
        {
            throw new CustomerAlreadyExistsException(customerId);
        }
        
        var customer = new Customer(customerId, @event.Email, _clock.CurrentDate());
        await _customerRepository.AddAsync(customer);
        await _messageBroker.PublishAsync(new CustomerCreated(customerId));
        _logger.LogInformation($"Created a customer with ID: '{customer.Id}'.");
    }
}