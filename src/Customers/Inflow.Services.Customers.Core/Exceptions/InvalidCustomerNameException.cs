using System;

namespace Inflow.Services.Customers.Core.Exceptions
{
    internal class InvalidCustomerNameException : CustomException
    {
        public Guid CustomerId { get; }

        public InvalidCustomerNameException(Guid customerId)
            : base($"Customer with ID: '{customerId}' has invalid name.")
        {
            CustomerId = customerId;
        }
    }
}