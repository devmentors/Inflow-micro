using Inflow.Services.Wallets.Infrastructure.EF;
using Inflow.Services.Wallets.Tests.Shared;

namespace Inflow.Services.Wallets.Tests.EndToEnd.Common
{
    internal class TestWalletsDbContext : TestDbContext<WalletsDbContext>
    {
        protected override WalletsDbContext Init(string connectionString)
            => new(DbHelper.GetOptions<WalletsDbContext>());
    }
}