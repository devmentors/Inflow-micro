using System;
using Convey;

namespace Inflow.Services.Users.Core.Infrastructure.Exceptions
{
    internal static class Extensions
    {
        public static string GetExceptionCode(this Exception exception)
            => exception.GetType().Name.Underscore().Replace("_exception", string.Empty);
    }
}