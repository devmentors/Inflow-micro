using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Withdrawals.Exceptions;

internal class CannotCompleteWithdrawalException : CustomException
{
    public Guid DepositId { get; }

    public CannotCompleteWithdrawalException(Guid depositId)
        : base($"Withdrawal with ID: '{depositId}' cannot be completed.")
    {
        DepositId = depositId;
    }
}