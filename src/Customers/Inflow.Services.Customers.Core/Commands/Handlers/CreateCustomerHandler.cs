﻿using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Inflow.Services.Customers.Core.Clients;
using Inflow.Services.Customers.Core.Domain.Entities;
using Inflow.Services.Customers.Core.Domain.Repositories;
using Inflow.Services.Customers.Core.Domain.ValueObjects;
using Inflow.Services.Customers.Core.Exceptions;
using Inflow.Services.Customers.Core.Services;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Commands.Handlers
{
    internal sealed class CreateCustomerHandler : ICommandHandler<CreateCustomer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IClock _clock;
        private readonly IUserApiClient _userApiClient;
        private readonly ILogger<CreateCustomerHandler> _logger;

        public CreateCustomerHandler(ICustomerRepository customerRepository, IClock clock,
           IUserApiClient userApiClient, ILogger<CreateCustomerHandler> logger)
        {
            _customerRepository = customerRepository;
            _clock = clock;
            _userApiClient = userApiClient;
            _logger = logger;
        }

        public async Task HandleAsync(CreateCustomer command)
        {
            _ = new Email(command.Email);
            var user = await _userApiClient.GetUserAsync(command.Email);
            if (user is null)
            {
                throw new UserNotFoundException(command.Email);
            }
            
            if (user.Role is not "user")
            {
                return;
            }

            var customerId = user.UserId;
            if (await _customerRepository.GetAsync(customerId) is not null)
            {
                throw new CustomerAlreadyExistsException(customerId);
            }

            var customer = new Customer(customerId, command.Email, _clock.CurrentDate());
            await _customerRepository.AddAsync(customer);
            _logger.LogInformation($"Created a customer with ID: '{customer.Id}'.");
        }
    }
}