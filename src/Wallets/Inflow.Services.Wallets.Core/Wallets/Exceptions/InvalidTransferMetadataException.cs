using Inflow.Services.Wallets.Core.Shared.Exceptions;

namespace Inflow.Services.Wallets.Core.Wallets.Exceptions;

internal class InvalidTransferMetadataException : DomainException
{
    public string Metadata { get; }

    public InvalidTransferMetadataException(string metadata) : base($"Transfer metadata: '{metadata}' is invalid.")
    {
        Metadata = metadata;
    }
}