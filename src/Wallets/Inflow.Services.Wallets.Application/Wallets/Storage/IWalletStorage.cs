using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Wallets.Core.Wallets.Entities;

namespace Inflow.Services.Wallets.Application.Wallets.Storage;

internal interface IWalletStorage
{
    Task<Wallet> FindAsync(Expression<Func<Wallet, bool>> expression);
    Task<PagedResult<Wallet>> BrowseAsync(Expression<Func<Wallet, bool>> expression, IPagedQuery query, CancellationToken cancellationToken = default);
}