using System;

namespace Inflow.Services.Wallets.Application.Exceptions
{
    internal abstract class AppException : Exception
    {
        protected AppException(string message) : base(message)
        {
        }
    }
}