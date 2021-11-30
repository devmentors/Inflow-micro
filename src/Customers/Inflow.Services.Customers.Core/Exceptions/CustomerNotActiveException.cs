using System;

namespace Inflow.Services.Customers.Core.Exceptions;

internal class CustomerNotActiveException : CustomException
{
    public Guid CustomerId { get; }

    public CustomerNotActiveException(Guid customerId)
        : base($"Customer with ID: '{customerId}' is not active.")
    {
        CustomerId = customerId;
    }
}