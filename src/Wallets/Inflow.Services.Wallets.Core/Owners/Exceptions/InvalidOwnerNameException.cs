using Inflow.Services.Wallets.Core.Shared.Exceptions;

namespace Inflow.Services.Wallets.Core.Owners.Exceptions
{
    internal class InvalidOwnerNameException : DomainException
    {
        public string Name { get; }

        public InvalidOwnerNameException(string name) : base($"Owner name: '{name}' is invalid.")
        {
            Name = name;
        }
    }
}