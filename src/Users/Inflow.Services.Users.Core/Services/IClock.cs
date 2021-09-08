using System;

namespace Inflow.Services.Users.Core.Services
{
    internal interface IClock
    {
        DateTime CurrentDate();
    }
}