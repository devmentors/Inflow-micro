using System.Threading.Tasks;
using Inflow.Services.Wallets.Core.Owners.Entities;
using Inflow.Services.Wallets.Core.Owners.Types;

namespace Inflow.Services.Wallets.Core.Owners.Repositories;

internal interface ICorporateOwnerRepository
{
    Task<CorporateOwner> GetAsync(OwnerId id);
    Task AddAsync(CorporateOwner owner);
    Task UpdateAsync(CorporateOwner owner);
}