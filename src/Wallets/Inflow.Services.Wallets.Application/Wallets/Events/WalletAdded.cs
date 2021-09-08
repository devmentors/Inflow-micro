using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Wallets.Application.Wallets.Events
{
    [Contract]
    internal record WalletAdded(Guid WalletId, Guid OwnerId, string Currency) : IEvent;
}