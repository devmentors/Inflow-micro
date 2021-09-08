using System;

namespace Inflow.Services.Users.Core.Services
{
    internal sealed class UtcClock : IClock
    {
        public DateTime CurrentDate()  => DateTime.UtcNow;
    }
}