using System.Threading.Tasks;
using Convey.CQRS.Events;
using Inflow.Services.Customers.Core.Domain.Entities;
using Inflow.Services.Customers.Core.Domain.Repositories;
using Inflow.Services.Customers.Core.Services;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Customers.Core.Events.External.Handlers
{
    internal sealed class SignedUpHandler : IEventHandler<SignedUp>
    {
        private const string ValidRole = "user";
        private readonly ICustomerRepository _customerRepository;
        private readonly IMessageBroker _messageBroker;
        private readonly IClock _clock;
        private readonly ILogger<SignedUpHandler> _logger;

        public SignedUpHandler(ICustomerRepository customerRepository, IMessageBroker messageBroker, IClock clock,
            ILogger<SignedUpHandler> logger)
        {
            _customerRepository = customerRepository;
            _messageBroker = messageBroker;
            _clock = clock;
            _logger = logger;
        }

        public async Task HandleAsync(SignedUp @event)
        {
            if (@event.Role is not ValidRole)
            {
                return;
            }

            var customer = new Customer(@event.UserId, @event.Email, _clock.CurrentDate());
            await _customerRepository.AddAsync(customer);
            _logger.LogInformation($"Created a new customer based on user with ID: '{@event.UserId}'.");
            await _messageBroker.PublishAsync(new CustomerCreated(customer.Id));
        }
    }
}