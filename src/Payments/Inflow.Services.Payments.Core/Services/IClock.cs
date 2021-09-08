using System;

namespace Inflow.Services.Payments.Core.Services
{
    internal interface IClock
    {
        DateTime CurrentDate();
    }
}