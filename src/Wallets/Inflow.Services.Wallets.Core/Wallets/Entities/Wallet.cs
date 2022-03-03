using System;
using System.Collections.Generic;
using Inflow.Services.Wallets.Core.Owners.Types;
using Inflow.Services.Wallets.Core.Shared.Types;
using Inflow.Services.Wallets.Core.Wallets.Types;
using Inflow.Services.Wallets.Core.Wallets.ValueObjects;

namespace Inflow.Services.Wallets.Core.Wallets.Entities;

internal class Wallet : AggregateRoot<WalletId>
{
    private HashSet<Transfer> _transfers = new();

    public OwnerId OwnerId { get; private set; }
    public Currency Currency { get; private set; }

    public IEnumerable<Transfer> Transfers
    {
        get => _transfers;
        set => _transfers = new HashSet<Transfer>(value);
    }

    public DateTime CreatedAt { get; private set; }

    private Wallet()
    {
    }

    public Wallet(WalletId id, OwnerId ownerId, Currency currency, DateTime createdAt)
    {
        Id = id;
        OwnerId = ownerId;
        Currency = currency;
        CreatedAt = createdAt;
    }
        
    public IReadOnlyCollection<Transfer> TransferFunds(Wallet receiver, Amount amount, DateTime createdAt)
    {
        var outTransferId = new TransferId();
        var inTransferId = new TransferId();

        var outTransfer = DeductFunds(outTransferId, amount, createdAt,
            metadata: GetMetadata(outTransferId, receiver.Id));
            
        var inTransfer = receiver.AddFunds(inTransferId, amount, createdAt,
            metadata: GetMetadata(inTransferId, Id));

        return new List<Transfer> { outTransfer, inTransfer };
            
        static TransferMetadata GetMetadata(TransferId referenceId, WalletId walletId)
            => new($"{{\"referenceId\": \"{referenceId}\", \"walletId\": \"{walletId}\"}}");
    }

    public IncomingTransfer AddFunds(TransferId transferId, Amount amount, DateTime createdAt,
        TransferName name = null, TransferMetadata metadata = null) => default;

    public OutgoingTransfer DeductFunds(TransferId transferId, Amount amount, DateTime createdAt,
        TransferName name = null, TransferMetadata metadata = null) => default;

    public Amount CurrentAmount() =>  default;
}