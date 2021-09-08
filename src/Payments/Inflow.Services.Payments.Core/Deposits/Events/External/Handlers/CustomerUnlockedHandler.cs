using System.Threading.Tasks;
using Convey.CQRS.Events;
using Inflow.Services.Payments.Shared.Exceptions;
using Inflow.Services.Payments.Shared.Repositories;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Payments.Core.Deposits.Events.External.Handlers
{
    internal sealed class CustomerUnlockedHandler : IEventHandler<CustomerUnlocked>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerUnlockedHandler> _logger;

        public CustomerUnlockedHandler(ICustomerRepository customerRepository,
            ILogger<CustomerUnlockedHandler> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }
        
        public async Task HandleAsync(CustomerUnlocked @event)
        {
            var customer = await _customerRepository.GetAsync(@event.CustomerId);
            if (customer is null)
            {
                throw new CustomerNotFoundException(@event.CustomerId);
            }
            
            customer.Unlock();
            await _customerRepository.UpdateAsync(customer);
        }
    }
}