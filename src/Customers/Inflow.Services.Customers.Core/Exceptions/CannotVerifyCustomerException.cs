using System;

namespace Inflow.Services.Customers.Core.Exceptions;

internal class CannotVerifyCustomerException : CustomException
{
    public Guid CustomerId { get; }

    public CannotVerifyCustomerException(Guid customerId)
        : base($"Customer with ID: '{customerId}' cannot be verified.")
    {
        CustomerId = customerId;
    }
}