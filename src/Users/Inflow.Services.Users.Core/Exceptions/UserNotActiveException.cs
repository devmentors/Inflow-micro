using System;

namespace Inflow.Services.Users.Core.Exceptions
{
    internal class UserNotActiveException : CustomException
    {
        public Guid UserId { get; }

        public UserNotActiveException(Guid userId) : base($"User with ID: '{userId}' is not active.")
        {
            UserId = userId;
        }
    }
}