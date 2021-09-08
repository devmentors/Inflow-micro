using System;

namespace Inflow.Services.Wallets.Application.Wallets.DTO
{
    public class WalletDto
    {
        public Guid WalletId { get; set; }
        public Guid OwnerId { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}