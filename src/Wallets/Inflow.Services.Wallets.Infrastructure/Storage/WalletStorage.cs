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

internal sealed class WalletStorage : IWalletStorage
{
    private readonly DbSet<Wallet> _wallets;

    public WalletStorage(WalletsDbContext dbContext)
    {
        _wallets = dbContext.Wallets;
    }

    public Task<Wallet> FindAsync(Expression<Func<Wallet, bool>> expression)
        => _wallets
            .AsNoTracking()
            .AsQueryable()
            .Where(expression)
            .Include(x => x.Transfers)
            .SingleOrDefaultAsync();

    public Task<PagedResult<Wallet>> BrowseAsync(Expression<Func<Wallet, bool>> expression, IPagedQuery query, CancellationToken cancellationToken = default)
        => _wallets
            .AsNoTracking()
            .AsQueryable()
            .Where(expression)
            .OrderBy(x => x.CreatedAt)
            .PaginateAsync(query, cancellationToken);
}