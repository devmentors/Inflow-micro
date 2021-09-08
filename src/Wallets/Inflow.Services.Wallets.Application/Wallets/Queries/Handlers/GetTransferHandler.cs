using System.Threading.Tasks;
using Convey.CQRS.Queries;
using Inflow.Services.Wallets.Application.Wallets.DTO;
using Inflow.Services.Wallets.Application.Wallets.Storage;

namespace Inflow.Services.Wallets.Application.Wallets.Queries.Handlers
{
    internal sealed class GetTransferHandler : IQueryHandler<GetTransfer, TransferDetailsDto>
    {
        private readonly ITransferStorage _storage;

        public GetTransferHandler(ITransferStorage storage)
        {
            _storage = storage;
        }

        public async Task<TransferDetailsDto> HandleAsync(GetTransfer query)
        {
            var transfer = await _storage.FindAsync(x => x.Id == query.TransferId);

            return transfer?.AsDetailsDto();
        }
    }
}