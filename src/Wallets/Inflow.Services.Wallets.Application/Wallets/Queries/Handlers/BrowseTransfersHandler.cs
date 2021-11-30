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

internal sealed class BrowseTransfersHandler : IQueryHandler<BrowseTransfers, PagedResult<TransferDto>>
{
    private readonly ITransferStorage _storage;

    public BrowseTransfersHandler(ITransferStorage storage)
    {
        _storage = storage;
    }

    public async Task<PagedResult<TransferDto>> HandleAsync(BrowseTransfers query, CancellationToken cancellationToken = default)
    {
        Expression<Func<Transfer, bool>> expression = x => true;

        if (!string.IsNullOrWhiteSpace(query.Currency))
        {
            _ = new Currency(query.Currency);
            expression = expression.And(x => x.Currency == query.Currency);
        }

        if (!string.IsNullOrWhiteSpace(query.Name))
        {
            expression = expression.And(x => x.Name == query.Name);
        }

        var result = await _storage.BrowseAsync(expression, query, cancellationToken);
        var transfers = result.Items.Select(x => x.AsDto()).ToList();

        return PagedResult<TransferDto>.From(result, transfers);
    }
}