using System;

namespace Inflow.Services.Wallets.Application.Services
{
    internal interface IClock
    {
        DateTime CurrentDate();
    }
}