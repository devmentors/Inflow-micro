namespace Inflow.Services.Payments.Shared.Exceptions
{
    internal class InvalidAmountException : CustomException
    {
        public decimal Amount { get; }

        public InvalidAmountException(decimal amount) : base($"Amount: '{amount}' is invalid.")
        {
            Amount = amount;
        }
    }
}