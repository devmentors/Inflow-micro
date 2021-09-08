using System;
using System.Linq;
using System.Threading.Tasks;
using Convey.CQRS.Commands;
using Inflow.Services.Wallets.Application.Services;
using Inflow.Services.Wallets.Application.Wallets.Commands;
using Inflow.Services.Wallets.Application.Wallets.Commands.Handlers;
using Inflow.Services.Wallets.Application.Wallets.Events;
using Inflow.Services.Wallets.Core.Owners.Entities;
using Inflow.Services.Wallets.Core.Owners.Repositories;
using Inflow.Services.Wallets.Core.Wallets.Entities;
using Inflow.Services.Wallets.Core.Wallets.Repositories;
using Inflow.Services.Wallets.Core.Wallets.ValueObjects;
using Inflow.Services.Wallets.Infrastructure.EF.Repositories;
using Inflow.Services.Wallets.Infrastructure.Time;
using Inflow.Services.Wallets.Tests.Integration.Common;
using Inflow.Services.Wallets.Tests.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Xunit;

namespace Inflow.Services.Wallets.Tests.Integration.Commands
{
    public class AddFundsHandlerTests : IDisposable
    {
        private Task Act(AddFunds command) => _handler.HandleAsync(command);
        
        [Fact]
        public async Task given_valid_command_add_funds_should_succeed()
        {
            await _dbContext.Context.Database.EnsureCreatedAsync();
            
            const decimal funds = 1000;
            const string currency = "EUR";
            var now = _clock.CurrentDate();
            var owner = new IndividualOwner(Guid.NewGuid(), "Owner 1", "John Doe 1", now);
            await _ownerRepository.AddAsync(owner);
            
            var wallet = new Wallet(Guid.NewGuid(), owner.Id, currency, now);
            await _walletRepository.AddAsync(wallet);

            var command = new AddFunds(wallet.Id, wallet.Currency, funds);
            await Act(command);

            var updatedWallet = await _walletRepository.GetAsync(wallet.Id);
            updatedWallet.CurrentAmount().ShouldBe(new Amount(funds));
            updatedWallet.Transfers.ShouldHaveSingleItem();
            var transfer = updatedWallet.Transfers.Single();
            transfer.ShouldBeOfType<IncomingTransfer>();
            
            _messageBroker.Events.ShouldNotBeEmpty();
            _messageBroker.Events.Count.ShouldBe(1);
            var @event = _messageBroker.Events[0];
            @event.ShouldBeOfType<FundsAdded>();
        }

        private readonly TestWalletsDbContext _dbContext;
        private readonly IIndividualOwnerRepository _ownerRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IClock _clock;
        private readonly ILogger<AddFundsHandler> _logger;
        private readonly TestMessageBroker _messageBroker;
        private readonly ICommandHandler<AddFunds> _handler;

        public AddFundsHandlerTests()
        {
            _dbContext = new TestWalletsDbContext();
            _ownerRepository = new IndividualOwnerRepository(_dbContext.Context);
            _walletRepository = new WalletRepository(_dbContext.Context);
            _clock = new UtcClock();
            _logger = new NullLogger<AddFundsHandler>();
            _messageBroker = new TestMessageBroker();
            _handler = new AddFundsHandler(_walletRepository, _clock, _messageBroker, _logger);
        }

        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}