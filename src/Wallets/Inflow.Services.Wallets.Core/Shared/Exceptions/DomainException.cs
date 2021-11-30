using System;

namespace Inflow.Services.Wallets.Core.Shared.Exceptions;

internal abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message)
    {
    }
}