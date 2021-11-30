using Convey.MessageBrokers.Outbox.Messages;
using Inflow.Services.Wallets.Core.Owners.Entities;
using Inflow.Services.Wallets.Core.Wallets.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Wallets.Infrastructure.EF;

internal class WalletsDbContext : DbContext
{
    public DbSet<InboxMessage> Inbox { get; set; }
    public DbSet<OutboxMessage> Outbox { get; set; }
    public DbSet<CorporateOwner> CorporateOwners { get; set; }
    public DbSet<IndividualOwner> IndividualOwners { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<IncomingTransfer> IncomingTransfers { get; set; }
    public DbSet<OutgoingTransfer> OutgoingTransfers { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
        
    public WalletsDbContext(DbContextOptions<WalletsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}