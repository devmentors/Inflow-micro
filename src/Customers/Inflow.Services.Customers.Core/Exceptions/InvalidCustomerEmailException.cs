using System;

namespace Inflow.Services.Customers.Core.Exceptions
{
    internal class InvalidCustomerEmailException : CustomException
    {
        public Guid CustomerId { get; }

        public InvalidCustomerEmailException(Guid customerId)
            : base($"Customer with ID: '{customerId}' has invalid email.")
        {
            CustomerId = customerId;
        }
    }
}