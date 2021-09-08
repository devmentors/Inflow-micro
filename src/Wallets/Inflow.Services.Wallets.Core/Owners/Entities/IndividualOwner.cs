using System;
using Inflow.Services.Wallets.Core.Owners.Types;
using Inflow.Services.Wallets.Core.Owners.ValueObjects;

namespace Inflow.Services.Wallets.Core.Owners.Entities
{
    internal class IndividualOwner : Owner
    {
        public FullName FullName { get; private set; }

        private IndividualOwner()
        {
        }
        
        public IndividualOwner(OwnerId id, OwnerName name, FullName fullName, DateTime createdAt) : base(id, name, createdAt)
        {
            FullName = fullName;
        }
    }
}