using System;
using Inflow.Services.Wallets.Core.Shared.Exceptions;

namespace Inflow.Services.Wallets.Application.Owners.Exceptions
{
    internal class OwnerNotFoundException : DomainException
    {
        public Guid OwnerId { get; }

        public OwnerNotFoundException(Guid ownerId) : base($"Owner with ID: '{ownerId}' was not found.")
        {
            OwnerId = ownerId;
        }
    }
}