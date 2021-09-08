using System;
using Inflow.Services.Wallets.Application.Services;

namespace Inflow.Services.Wallets.Infrastructure.Time
{
    internal sealed class UtcClock : IClock
    {
        public DateTime CurrentDate()  => DateTime.UtcNow;
    }
}