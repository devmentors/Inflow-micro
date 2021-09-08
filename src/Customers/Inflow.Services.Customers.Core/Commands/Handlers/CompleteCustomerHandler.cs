using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Inflow.Services.Customers.Core.Domain.Repositories;
using Inflow.Services.Customers.Core.Domain.ValueObjects;
using Inflow.Services.Customers.Core.Events;
using Inflow.Services.Customers.Core.Exceptions;
using Inflow.Services.Customers.Core.Services;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Commands.Handlers
{
    internal sealed class CompleteCustomerHandler : ICommandHandler<CompleteCustomer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IClock _clock;
        private readonly ILogger<CompleteCustomerHandler> _logger;

        public CompleteCustomerHandler(ICustomerRepository customerRepository, IMessageBroker messageBroker,
            IClock clock, ILogger<CompleteCustomerHandler> logger)
        {
            _customerRepository = customerRepository;
            _messageBroker = messageBroker;
            _clock = clock;
            _logger = logger;
        }

        public async Task HandleAsync(CompleteCustomer command)
        {
            var customer = await _customerRepository.GetAsync(command.CustomerId);
            if (customer is null)
            {
                throw new CustomerNotFoundException(command.CustomerId);
            }

            if (!string.IsNullOrWhiteSpace(command.Name) && await _customerRepository.ExistsAsync(command.Name))
            {
                throw new CustomerAlreadyExistsException(command.Name);
            }

            customer.Complete(command.Name, command.FullName, command.Address, command.Nationality,
                new Identity(command.IdentityType, command.IdentitySeries), _clock.CurrentDate());
            await _customerRepository.UpdateAsync(customer);
            await _messageBroker.PublishAsync(new CustomerCompleted(customer.Id, customer.Name, customer.FullName,
                customer.Nationality));
            _logger.LogInformation($"Completed a customer with ID: '{command.CustomerId}'.");
        }
    }
}