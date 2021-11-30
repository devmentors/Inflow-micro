using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Withdrawals.Exceptions;

internal class WithdrawalNotFoundException : CustomException
{
    public Guid DepositId { get; }

    public WithdrawalNotFoundException(Guid depositId) : base($"Withdrawal with ID: '{depositId}' was not found.")
    {
        DepositId = depositId;
    }
}