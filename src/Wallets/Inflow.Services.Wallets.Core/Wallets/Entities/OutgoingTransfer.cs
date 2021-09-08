using System;
using Inflow.Services.Wallets.Core.Wallets.Types;
using Inflow.Services.Wallets.Core.Wallets.ValueObjects;

namespace Inflow.Services.Wallets.Core.Wallets.Entities
{
    internal class OutgoingTransfer : Transfer
    {
        protected OutgoingTransfer()
        {
        }

        public OutgoingTransfer(TransferId id, WalletId walletId, Currency currency, Amount amount, DateTime createdAt,
            TransferName name = null, TransferMetadata metadata = null) : base(id, walletId, currency, amount,
            createdAt, name, metadata)
        {
        }
    }
}