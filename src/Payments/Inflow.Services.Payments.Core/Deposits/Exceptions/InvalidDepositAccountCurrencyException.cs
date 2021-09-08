using System;
using Inflow.Services.Payments.Shared.Exceptions;

namespace Inflow.Services.Payments.Core.Deposits.Exceptions
{
    internal class InvalidDepositAccountCurrencyException : CustomException
    {
        public Guid AccountId { get; }
        public string Currency { get; }

        public InvalidDepositAccountCurrencyException(Guid accountId, string currency)
            : base($"Deposit account with ID: '{accountId}' has invalid currency: '{currency}'.")
        {
            AccountId = accountId;
            Currency = currency;
        }
    }
}