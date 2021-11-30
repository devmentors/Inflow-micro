using System.Collections.Generic;

namespace Inflow.Services.Wallets.Application.Wallets.DTO;

public class WalletDetailsDto : WalletDto
{
    public decimal Amount { get; set; }
    public List<TransferDto> Transfers { get; set; }
}