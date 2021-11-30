using System;
using Inflow.Services.Wallets.Core.Owners.Types;
using Inflow.Services.Wallets.Core.Shared.ValueObjects;

namespace Inflow.Services.Wallets.Core.Wallets.Types;

internal class WalletId : TypeId
{
    public WalletId() : this(Guid.NewGuid())
    {
    }
        
    public WalletId(Guid value) : base(value)
    {
    }
        
    public static implicit operator WalletId(Guid id) => new(id);
        
    public static implicit operator WalletId(OwnerId id) => id.Value;
        
    public override string ToString() => Value.ToString();
}