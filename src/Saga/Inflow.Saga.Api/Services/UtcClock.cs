using System;

namespace Inflow.Saga.Api.Services
{
    internal sealed class UtcClock : IClock
    {
        public DateTime CurrentDate()  => DateTime.UtcNow;
    }
}