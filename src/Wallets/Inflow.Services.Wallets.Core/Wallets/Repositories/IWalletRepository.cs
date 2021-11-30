using System.Threading.Tasks;
using Inflow.Services.Wallets.Core.Owners.Types;
using Inflow.Services.Wallets.Core.Wallets.Entities;
using Inflow.Services.Wallets.Core.Wallets.Types;
using Inflow.Services.Wallets.Core.Wallets.ValueObjects;

namespace Inflow.Services.Wallets.Core.Wallets.Repositories;

internal interface IWalletRepository
{
    Task<Wallet> GetAsync(WalletId id);
    Task<Wallet> GetAsync(OwnerId ownerId, Currency currency);
    Task AddAsync(Wallet wallet);
    Task UpdateAsync(Wallet wallet);
}