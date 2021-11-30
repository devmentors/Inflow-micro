using System;

namespace Inflow.Services.Customers.Core.Exceptions;

internal abstract class CustomException : Exception
{
    protected CustomException(string message) : base(message)
    {
    }
}