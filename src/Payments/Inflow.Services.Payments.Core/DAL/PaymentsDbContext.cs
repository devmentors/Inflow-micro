using Convey.MessageBrokers.Outbox.Messages;
using Inflow.Services.Payments.Core.Deposits.Domain.Entities;
using Inflow.Services.Payments.Core.Withdrawals.Domain.Entities;
using Inflow.Services.Payments.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Payments.Core.DAL;

internal class PaymentsDbContext : DbContext
{
    public DbSet<InboxMessage> Inbox { get; set; }
    public DbSet<OutboxMessage> Outbox { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<DepositAccount> DepositAccounts { get; set; }
    public DbSet<Deposit> Deposits { get; set; }
    public DbSet<Withdrawal> Withdrawals { get; set; }
    public DbSet<WithdrawalAccount> WithdrawalAccounts { get; set; }
        
    public PaymentsDbContext(DbContextOptions<PaymentsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}