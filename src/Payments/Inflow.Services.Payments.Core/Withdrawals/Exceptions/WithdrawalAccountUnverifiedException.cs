using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Withdrawals.Exceptions;

internal class WithdrawalAccountUnverifiedException : CustomException
{
    public Guid AccountId { get; }
    public Guid CustomerId { get; }

    public WithdrawalAccountUnverifiedException(Guid accountId, Guid customerId)
        : base($"Withdrawal account with ID: '{accountId}' for customer with ID: '{customerId}' is unverified.")
    {
        AccountId = accountId;
        CustomerId = customerId;
    }
}