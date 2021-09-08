using Inflow.Services.Wallets.Core.Shared.Exceptions;

namespace Inflow.Services.Wallets.Core.Wallets.Exceptions
{
    internal class InvalidTransferCurrencyException : DomainException
    {
        public string Currency { get; }

        public InvalidTransferCurrencyException(string currency) : base($"Transfer has invalid currency: '{currency}'.")
        {
            Currency = currency;
        }
    }
}