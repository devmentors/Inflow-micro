namespace Inflow.Services.Customers.Core.Exceptions
{
    internal class UnsupportedNationalityException : CustomException
    {
        public string Nationality { get; }

        public UnsupportedNationalityException(string nationality) : base($"Nationality: '{nationality}' is unsupported.")
        {
            Nationality = nationality;
        }
    }
}