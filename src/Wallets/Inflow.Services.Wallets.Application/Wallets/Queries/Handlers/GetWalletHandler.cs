using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Wallets.Application.Contexts;
using Inflow.Services.Wallets.Application.Wallets.DTO;
using Inflow.Services.Wallets.Application.Wallets.Storage;

namespace Inflow.Services.Wallets.Application.Wallets.Queries.Handlers
{
    internal sealed class GetWalletHandler : IQueryHandler<GetWallet, WalletDetailsDto>
    {
        private readonly IWalletStorage _storage;
        private readonly IContext _context;

        public GetWalletHandler(IWalletStorage storage, IContext context)
        {
            _storage = storage;
            _context = context;
        }

        public async Task<WalletDetailsDto> HandleAsync(GetWallet query)
        {
            // Owner cannot access the other wallets
            var wallet = await _storage.FindAsync(x => x.Id == query.WalletId);
            if (wallet is null || _context.Identity.IsUser() && _context.Identity.Id != wallet.OwnerId)
            {
                return null;
            }
            
            return wallet.AsDetailsDto();
        }
    }
}