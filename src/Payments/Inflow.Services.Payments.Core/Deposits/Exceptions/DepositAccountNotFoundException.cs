using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Deposits.Exceptions
{
    internal class DepositAccountNotFoundException : CustomException
    {
        public Guid AccountId { get; }
        public Guid CustomerId { get; }

        public DepositAccountNotFoundException(Guid accountId, Guid customerId)
            : base($"Deposit account with ID: '{accountId}' for customer with ID: '{customerId}' was not found.")
        {
            AccountId = accountId;
            CustomerId = customerId;
        }
    }
}