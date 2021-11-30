using System;

namespace Inflow.Services.Payments.Core.Services;

internal sealed class UtcClock : IClock
{
    public DateTime CurrentDate()  => DateTime.UtcNow;
}