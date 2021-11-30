using System.Threading;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Inflow.Services.Wallets.Application.Services;
using Inflow.Services.Wallets.Application.Wallets.Events;
using Inflow.Services.Wallets.Application.Wallets.Exceptions;
using Inflow.Services.Wallets.Core.Wallets.Exceptions;
using Inflow.Services.Wallets.Core.Wallets.Repositories;
using Microsoft.Extensions.Logging;

namespace Inflow.Services.Wallets.Application.Wallets.Commands.Handlers;

internal sealed class DeductFundsHandler : ICommandHandler<DeductFunds>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IClock _clock;
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger<DeductFundsHandler> _logger;

    public DeductFundsHandler(IWalletRepository walletRepository, IClock clock, IMessageBroker messageBroker,
        ILogger<DeductFundsHandler> logger)
    {
        _walletRepository = walletRepository;
        _clock = clock;
        _messageBroker = messageBroker;
        _logger = logger;
    }

    public async Task HandleAsync(DeductFunds command, CancellationToken cancellationToken = default)
    {
        var wallet = await _walletRepository.GetAsync(command.WalletId);
        if (wallet is null)
        {
            throw new WalletNotFoundException(command.WalletId);
        }

        if (wallet.Currency != command.Currency)
        {
            throw new InvalidTransferCurrencyException(command.Currency);
        }

        var transfer = wallet.DeductFunds(command.TransferId, command.Amount, _clock.CurrentDate(),
            command.TransferName, command.TransferMetadata);
        await _walletRepository.UpdateAsync(wallet);
        await _messageBroker.PublishAsync(new FundsDeducted(wallet.Id, wallet.OwnerId, wallet.Currency,
            transfer.Amount, transfer.Name, transfer.Metadata));
        _logger.LogInformation($"Deducted {transfer.Amount} {transfer.Currency} from wallet with ID: '{wallet.Id}'.");
    }
}