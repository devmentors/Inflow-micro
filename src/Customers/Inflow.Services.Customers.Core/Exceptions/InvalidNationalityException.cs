namespace Inflow.Services.Customers.Core.Exceptions
{
    internal class InvalidNationalityException : CustomException
    {
        public string Nationality { get; }

        public InvalidNationalityException(string nationality) : base($"Nationality: '{nationality}' is invalid.")
        {
            Nationality = nationality;
        }
    }
}