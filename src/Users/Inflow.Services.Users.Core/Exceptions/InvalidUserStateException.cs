namespace Inflow.Services.Users.Core.Exceptions
{
    internal class InvalidUserStateException : CustomException
    {
        public string State { get; }

        public InvalidUserStateException(string state) : base($"User state is invalid: '{state}'.")
        {
            State = state;
        }
    }
}