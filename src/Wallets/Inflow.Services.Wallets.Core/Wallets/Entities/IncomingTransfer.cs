using System;
using Inflow.Services.Wallets.Core.Wallets.Types;
using Inflow.Services.Wallets.Core.Wallets.ValueObjects;

namespace Inflow.Services.Wallets.Core.Wallets.Entities
{
    internal class IncomingTransfer : Transfer
    {
        protected IncomingTransfer()
        {
        }

        public IncomingTransfer(TransferId id, WalletId walletId, Currency currency, Amount amount, DateTime createdAt,
            TransferName name = null, TransferMetadata metadata = null) : base(id, walletId, currency, amount,
            createdAt, name, metadata)
        {
        }
    }
}