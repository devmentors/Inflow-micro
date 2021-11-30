using System;
using Inflow.Services.Wallets.Core.Shared.ValueObjects;

namespace Inflow.Services.Wallets.Core.Owners.Types;

internal class OwnerId : TypeId
{
    public OwnerId(Guid value) : base(value)
    {
    }
        
    public static implicit operator OwnerId(Guid id) => new(id);
        
    public static implicit operator Guid(OwnerId id) => id.Value;
        
    public override string ToString() => Value.ToString();
}