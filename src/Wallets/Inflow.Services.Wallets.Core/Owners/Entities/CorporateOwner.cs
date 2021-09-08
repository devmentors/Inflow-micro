using System;
using Inflow.Services.Wallets.Core.Owners.Types;
using Inflow.Services.Wallets.Core.Owners.ValueObjects;

namespace Inflow.Services.Wallets.Core.Owners.Entities
{
    internal class CorporateOwner : Owner
    {
        public TaxId TaxId { get; private set; }
        
        private CorporateOwner()
        {
        }
        
        public CorporateOwner(OwnerId id, OwnerName name, TaxId taxId, DateTime createdAt) : base(id, name, createdAt)
        {
            TaxId = taxId;
        }
    }
}