using Inflow.Services.Wallets.Core.Shared.Exceptions;

namespace Inflow.Services.Wallets.Core.Owners.Exceptions;

internal class InvalidTaxIdException : DomainException
{
    public string TaxId { get; }

    public InvalidTaxIdException(string taxId) : base($"Tax ID: '{taxId}' is invalid.")
    {
        TaxId = taxId;
    }
}