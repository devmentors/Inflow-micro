using Inflow.Services.Payments.Shared.ValueObjects;

namespace Inflow.Services.Payments.Core.Deposits.Domain.Services
{
    internal interface ICurrencyResolver
    {
        Currency GetForNationality(Nationality nationality);
    }
}