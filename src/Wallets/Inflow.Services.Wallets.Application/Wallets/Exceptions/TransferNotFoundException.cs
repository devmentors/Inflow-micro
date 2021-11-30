using System;
using Inflow.Services.Wallets.Application.Exceptions;

namespace Inflow.Services.Wallets.Application.Wallets.Exceptions;

internal class TransferNotFoundException : AppException
{
    public Guid TransferId { get; }

    public TransferNotFoundException(Guid transferId) : base($"Transfer with ID: '{transferId}' was not found.")
    {
        TransferId = transferId;
    }
}