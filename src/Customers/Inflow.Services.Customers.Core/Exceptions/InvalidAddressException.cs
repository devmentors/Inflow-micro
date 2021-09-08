namespace Inflow.Services.Customers.Core.Exceptions
{
    internal class InvalidAddressException : CustomException
    {
        public string Address { get; }

        public InvalidAddressException(string address) : base($"Address: '{address}' is invalid.")
        {
            Address = address;
        }
    }
}