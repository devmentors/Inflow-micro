using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Wallets.Application.Wallets.DTO;
using Inflow.Services.Wallets.Application.Wallets.Storage;
using Inflow.Services.Wallets.Core.Wallets.Entities;
using Inflow.Services.Wallets.Core.Wallets.ValueObjects;

namespace Inflow.Services.Wallets.Application.Wallets.Queries.Handlers;

internal sealed class BrowseWalletsHandler : IQueryHandler<BrowseWallets, PagedResult<WalletDto>>
{
    private readonly IWalletStorage _storage;

    public BrowseWalletsHandler(IWalletStorage storage)
    {
        _storage = storage;
    }

    public async Task<PagedResult<WalletDto>> HandleAsync(BrowseWallets query, CancellationToken cancellationToken = default)
    {
        Expression<Func<Wallet, bool>> expression = x => true;

        if (!string.IsNullOrWhiteSpace(query.Currency))
        {
            _ = new Currency(query.Currency);
            expression = expression.And(x => x.Currency == query.Currency);
        }

        if (query.OwnerId.HasValue)
        {
            expression = expression.And(x => x.OwnerId == query.OwnerId);
        }

        var result = await _storage.BrowseAsync(expression, query, cancellationToken);
        var wallets = result.Items.Select(x => x.AsDto()).ToList();

        return PagedResult<WalletDto>.From(result, wallets);
    }
}