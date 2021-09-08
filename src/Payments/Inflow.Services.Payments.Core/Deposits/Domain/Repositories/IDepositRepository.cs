using System;
using System.Threading.Tasks;
using Inflow.Services.Payments.Core.Deposits.Domain.Entities;

namespace Inflow.Services.Payments.Core.Deposits.Domain.Repositories
{
    internal interface IDepositRepository
    {
        Task<Deposit> GetAsync(Guid id);
        Task AddAsync(Deposit deposit);
        Task UpdateAsync(Deposit deposit);
    }
}