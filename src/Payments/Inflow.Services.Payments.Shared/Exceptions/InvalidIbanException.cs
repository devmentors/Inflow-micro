namespace Inflow.Services.Payments.Shared.Exceptions;

internal class InvalidIbanException : CustomException
{
    public string Iban { get; }

    public InvalidIbanException(string iban) : base($"IBAN: '{iban}' is invalid.")
    {
        Iban = iban;
    }
}