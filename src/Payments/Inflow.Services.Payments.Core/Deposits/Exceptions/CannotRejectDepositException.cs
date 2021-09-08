using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Deposits.Exceptions
{
    internal class CannotRejectDepositException : CustomException
    {
        public Guid DepositId { get; }

        public CannotRejectDepositException(Guid depositId)
            : base($"Deposit with ID: '{depositId}' cannot be rejected.")
        {
            DepositId = depositId;
        }
    }
}