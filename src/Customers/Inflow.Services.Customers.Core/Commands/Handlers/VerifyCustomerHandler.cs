using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Inflow.Services.Customers.Core.Domain.Repositories;
using Inflow.Services.Customers.Core.Events;
using Inflow.Services.Customers.Core.Exceptions;
using Inflow.Services.Customers.Core.Services;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Commands.Handlers
{
    internal sealed class VerifyCustomerHandler : ICommandHandler<VerifyCustomer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IClock _clock;
        private readonly ILogger<VerifyCustomerHandler> _logger;

        public VerifyCustomerHandler(ICustomerRepository customerRepository, IMessageBroker messageBroker,
            IClock clock, ILogger<VerifyCustomerHandler> logger)
        {
            _customerRepository = customerRepository;
            _messageBroker = messageBroker;
            _clock = clock;
            _logger = logger;
        }

        public async Task HandleAsync(VerifyCustomer command)
        {
            var customer = await _customerRepository.GetAsync(command.CustomerId);
            if (customer is null)
            {
                throw new CustomerNotFoundException(command.CustomerId);
            }

            customer.Verify(_clock.CurrentDate());
            await _customerRepository.UpdateAsync(customer);
            await _messageBroker.PublishAsync(new CustomerVerified(command.CustomerId));
            _logger.LogInformation($"Verified a customer with ID: '{command.CustomerId}'.");
        }
    }
}