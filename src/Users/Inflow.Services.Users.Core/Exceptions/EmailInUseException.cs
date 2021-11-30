namespace Inflow.Services.Users.Core.Exceptions;

internal class EmailInUseException : CustomException
{
    public EmailInUseException() : base("Email is already in use.")
    {
    }
}