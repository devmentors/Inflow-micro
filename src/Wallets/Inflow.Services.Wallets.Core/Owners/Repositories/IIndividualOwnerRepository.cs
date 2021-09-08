using System.Threading.Tasks;
using Inflow.Services.Wallets.Core.Owners.Entities;
using Inflow.Services.Wallets.Core.Owners.Types;

namespace Inflow.Services.Wallets.Core.Owners.Repositories
{
    internal interface IIndividualOwnerRepository
    {
        Task<IndividualOwner> GetAsync(OwnerId id);
        Task AddAsync(IndividualOwner owner);
        Task UpdateAsync(IndividualOwner owner);
    }
}