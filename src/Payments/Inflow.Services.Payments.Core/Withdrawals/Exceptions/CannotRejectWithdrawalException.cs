using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Withdrawals.Exceptions;

internal class CannotRejectWithdrawalException : CustomException
{
    public Guid DepositId { get; }

    public CannotRejectWithdrawalException(Guid depositId)
        : base($"Withdrawal with ID: '{depositId}' cannot be rejected.")
    {
        DepositId = depositId;
    }
}