using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Inflow.Services.Customers.Core.Domain.Repositories;
using Inflow.Services.Customers.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Commands.Handlers;

internal sealed class UnlockCustomerHandler : ICommandHandler<UnlockCustomer>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<UnlockCustomerHandler> _logger;

    public UnlockCustomerHandler(ICustomerRepository customerRepository, ILogger<UnlockCustomerHandler> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }
        
    public async Task HandleAsync(UnlockCustomer command, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetAsync(command.CustomerId);
        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }
            
        customer.Unlock(command.Notes);
        await _customerRepository.UpdateAsync(customer);
        _logger.LogInformation($"Unlocked a customer with ID: '{command.CustomerId}'.");
    }
}