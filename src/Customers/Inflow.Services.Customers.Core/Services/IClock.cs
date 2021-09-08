using System;

namespace Inflow.Services.Customers.Core.Services
{
    internal interface IClock
    {
        DateTime CurrentDate();
    }
}