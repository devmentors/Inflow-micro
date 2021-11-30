namespace Inflow.Services.Users.Core.Exceptions;

internal class InvalidPasswordException : CustomException
{
    public string Reason { get; }

    public InvalidPasswordException(string reason) : base($"Invalid password: {reason}.")
    {
        Reason = reason;
    }
}