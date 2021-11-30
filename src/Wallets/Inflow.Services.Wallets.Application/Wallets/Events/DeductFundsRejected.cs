using System;
using Convey.CQRS.Events;

namespace Inflow.Services.Wallets.Application.Wallets.Events;

[Contract]
internal record DeductFundsRejected(Guid WalletId, Guid OwnerId, string Currency, decimal Amount,
    string TransferName = null, string TransferMetadata = null) : IEvent;