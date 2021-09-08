namespace Inflow.Services.Users.Core.Exceptions
{
    internal class SignUpDisabledException : CustomException
    {
        public SignUpDisabledException() : base("Sign up is disabled.")
        {
        }
    }
}