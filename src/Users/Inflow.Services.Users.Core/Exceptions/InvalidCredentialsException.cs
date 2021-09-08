namespace Inflow.Services.Users.Core.Exceptions
{
    internal class InvalidCredentialsException : CustomException
    {
        public InvalidCredentialsException() : base("Invalid credentials.")
        {
        }
    }
}