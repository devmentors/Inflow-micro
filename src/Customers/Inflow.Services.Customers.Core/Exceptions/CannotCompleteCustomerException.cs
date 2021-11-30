using System;

namespace Inflow.Services.Customers.Core.Exceptions;

internal class CannotCompleteCustomerException : CustomException
{
    public Guid CustomerId { get; }

    public CannotCompleteCustomerException(Guid customerId)
        : base($"Customer with ID: '{customerId}' cannot be completed.")
    {
        CustomerId = customerId;
    }
}