using Inflow.Services.Payments.Shared.ValueObjects;

namespace Inflow.Services.Payments.Core.Deposits.Domain.Services
{
    internal class CurrencyResolver : ICurrencyResolver
    {
        public Currency GetForNationality(Nationality nationality)
            => nationality.Value switch
            {
                "PL" => "PLN",
                "DE" => "EUR",
                "FR" => "EUR",
                "ES" => "EUR",
                "GB" => "GBP",
                _ => "EUR"
            };
    }
}