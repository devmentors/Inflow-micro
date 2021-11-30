using System;
using System.Threading.Tasks;
using Inflow.Services.Payments.Core.Withdrawals.Domain.Entities;
using Inflow.Services.Payments.Shared.ValueObjects;

namespace Inflow.Services.Payments.Core.Withdrawals.Domain.Repositories;

internal interface IWithdrawalAccountRepository
{
    Task<bool> ExistsAsync(Guid customerId, Currency currency);
    Task<WithdrawalAccount> GetAsync(Guid id);
    Task<WithdrawalAccount> GetAsync(Guid customerId, Currency currency);
    Task AddAsync(WithdrawalAccount withdrawalAccount);
}