using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Withdrawals.Exceptions
{
    internal class WithdrawalAccountNotFoundException : CustomException
    {
        public Guid AccountId { get; }
        public Guid CustomerId { get; }

        public WithdrawalAccountNotFoundException(Guid accountId, Guid customerId)
            : base($"Withdrawal account with ID: '{accountId}' for customer with ID: '{customerId}' was not found.")
        {
            AccountId = accountId;
            CustomerId = customerId;
        }
    }
}