using System;

namespace Inflow.Services.Payments.Shared.Exceptions;

internal abstract class CustomException : Exception
{
    protected CustomException(string message) : base(message)
    {
    }
}