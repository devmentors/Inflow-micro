using System;

namespace Inflow.Services.Customers.Core.Services
{
    internal sealed class UtcClock : IClock
    {
        public DateTime CurrentDate()  => DateTime.UtcNow;
    }
}