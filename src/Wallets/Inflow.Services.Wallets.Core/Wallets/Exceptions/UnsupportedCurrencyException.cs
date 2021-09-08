using Inflow.Services.Wallets.Core.Shared.Exceptions;

namespace Inflow.Services.Wallets.Core.Wallets.Exceptions
{
    internal class UnsupportedCurrencyException : DomainException
    {
        public string Currency { get; }

        public UnsupportedCurrencyException(string currency) : base($"Currency: '{currency}' is unsupported.")
        {
            Currency = currency;
        }
    }
}