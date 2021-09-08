namespace Inflow.Services.Payments.Shared.Exceptions
{
    internal class UnsupportedCurrencyException : CustomException
    {
        public string Currency { get; }

        public UnsupportedCurrencyException(string currency) : base($"Currency: '{currency}' is unsupported.")
        {
            Currency = currency;
        }
    }
}