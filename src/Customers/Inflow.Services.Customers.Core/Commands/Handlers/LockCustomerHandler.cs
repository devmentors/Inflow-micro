﻿using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Inflow.Services.Customers.Core.Domain.Repositories;
using Inflow.Services.Customers.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Commands.Handlers;

internal sealed class LockCustomerHandler : ICommandHandler<LockCustomer>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<LockCustomerHandler> _logger;

    public LockCustomerHandler(ICustomerRepository customerRepository, ILogger<LockCustomerHandler> logger)
    {
        _customerRepository = customerRepository;
        _logger = logger;
    }
        
    public async Task HandleAsync(LockCustomer command, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetAsync(command.CustomerId);
        if (customer is null)
        {
            throw new CustomerNotFoundException(command.CustomerId);
        }
            
        customer.Lock(command.Notes);
        await _customerRepository.UpdateAsync(customer);
        _logger.LogInformation($"Locked a customer with ID: '{command.CustomerId}'.");
    }
}