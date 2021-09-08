using System;

namespace Inflow.Saga.Api.Services
{
    internal interface IClock
    {
        DateTime CurrentDate();
    }
}