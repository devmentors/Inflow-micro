using System;

namespace Inflow.Services.Customers.Core.Exceptions;

internal class CustomerLockedException : CustomException
{
    public Guid CustomerId { get; }

    public CustomerLockedException(Guid customerId)
        : base($"Customer with ID: '{customerId}' is locked.")
    {
        CustomerId = customerId;
    }
}