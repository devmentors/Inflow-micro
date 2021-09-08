using System;

namespace Inflow.Services.Payments.Shared.Exceptions
{
    internal class CustomerNotFoundException : CustomException
    {
        public Guid CustomerId { get; }

        public CustomerNotFoundException(Guid customerId)
            : base($"Customer with ID: '{customerId}' was not found.")
        {
            CustomerId = customerId;
        }
    }
}