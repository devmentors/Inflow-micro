using System;
using Convey.CQRS.Queries;
using Inflow.Services.Wallets.Application.Wallets.DTO;

namespace Inflow.Services.Wallets.Application.Wallets.Queries
{
    public class GetTransfer : IQuery<TransferDetailsDto>
    {
        public Guid? TransferId { get; set; }
    }
}