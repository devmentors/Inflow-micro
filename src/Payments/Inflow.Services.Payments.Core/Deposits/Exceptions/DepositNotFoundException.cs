using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Deposits.Exceptions
{
    internal class DepositNotFoundException : CustomException
    {
        public Guid DepositId { get; }

        public DepositNotFoundException(Guid depositId) : base($"Deposit with ID: '{depositId}' was not found.")
        {
            DepositId = depositId;
        }
    }
}