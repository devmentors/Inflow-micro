using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Wallets.Application.Wallets.Storage;
using Inflow.Services.Wallets.Core.Wallets.Entities;
using Inflow.Services.Wallets.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Wallets.Infrastructure.Storage;

internal sealed class TransferStorage : ITransferStorage
{
    private readonly DbSet<Transfer> _transfers;

    public TransferStorage(WalletsDbContext dbContext)
    {
        _transfers = dbContext.Transfers;
    }

    public Task<Transfer> FindAsync(Expression<Func<Transfer, bool>> expression)
        => _transfers
            .AsNoTracking()
            .AsQueryable()
            .Where(expression)
            .SingleOrDefaultAsync();

    public Task<PagedResult<Transfer>> BrowseAsync(Expression<Func<Transfer, bool>> expression, IPagedQuery query, CancellationToken cancellationToken = default)
        => _transfers
            .AsNoTracking()
            .AsQueryable()
            .Where(expression)
            .OrderBy(x => x.CreatedAt)
            .PaginateAsync(query, cancellationToken);
}