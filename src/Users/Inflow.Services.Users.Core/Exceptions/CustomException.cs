using System;

namespace Inflow.Services.Users.Core.Exceptions
{
    internal abstract class CustomException : Exception
    {
        protected CustomException(string message) : base(message)
        {
        }
    }
}