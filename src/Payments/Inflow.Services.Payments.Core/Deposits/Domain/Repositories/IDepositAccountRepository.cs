using System;
using System.Threading.Tasks;
using Inflow.Services.Payments.Core.Deposits.Domain.Entities;
using Inflow.Services.Payments.Shared.ValueObjects;

namespace Inflow.Services.Payments.Core.Deposits.Domain.Repositories
{
    internal interface IDepositAccountRepository
    {
        Task<DepositAccount> GetAsync(Guid id);
        Task<DepositAccount> GetAsync(Guid customerId, Currency currency);
        Task AddAsync(DepositAccount depositAccount);
    }
}