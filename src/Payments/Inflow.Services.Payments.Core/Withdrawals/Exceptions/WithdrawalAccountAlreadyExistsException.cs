using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Withdrawals.Exceptions;

internal class WithdrawalAccountAlreadyExistsException : CustomException
{
    public Guid CustomerId { get; }
    public string Currency { get; }

    public WithdrawalAccountAlreadyExistsException(Guid customerId, string currency)
        : base($"Withdrawal account for customer with ID: '{customerId}', currency: '{currency}' already exists.")
    {
        CustomerId = customerId;
        Currency = currency;
    }
}