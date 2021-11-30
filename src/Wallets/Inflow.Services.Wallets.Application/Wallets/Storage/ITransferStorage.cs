using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Wallets.Core.Wallets.Entities;

namespace Inflow.Services.Wallets.Application.Wallets.Storage;

internal interface ITransferStorage
{
    Task<Transfer> FindAsync(Expression<Func<Transfer, bool>> expression);
    Task<PagedResult<Transfer>> BrowseAsync(Expression<Func<Transfer, bool>> expression, IPagedQuery query, CancellationToken cancellationToken = default);
}