using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Deposits.Exceptions
{
    internal class DepositAccountAlreadyExistsException : CustomException
    {
        public Guid CustomerId { get; }
        public string Currency { get; }

        public DepositAccountAlreadyExistsException(Guid customerId, string currency)
            : base($"Deposit account for customer with ID: '{customerId}', currency: '{currency}' already exists.")
        {
            CustomerId = customerId;
            Currency = currency;
        }
    }
}