using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Deposits.Exceptions
{
    internal class CannotCompleteDepositException : CustomException
    {
        public Guid DepositId { get; }

        public CannotCompleteDepositException(Guid depositId)
            : base($"Deposit with ID: '{depositId}' cannot be completed.")
        {
            DepositId = depositId;
        }
    }
}