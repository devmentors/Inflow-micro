using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Deposits.Exceptions;

internal class DepositAccountUnverifiedException : CustomException
{
    public Guid AccountId { get; }
    public Guid CustomerId { get; }

    public DepositAccountUnverifiedException(Guid accountId, Guid customerId)
        : base($"Deposit account with ID: '{accountId}' for customer with ID: '{customerId}' is unverified.")
    {
        AccountId = accountId;
        CustomerId = customerId;
    }
}