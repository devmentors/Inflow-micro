using System;
using System.Collections.Concurrent;
using System.Net;
using Convey.WebApi.Exceptions;
using Inflow.Services.Customers.Core.Exceptions;

namespace Inflow.Services.Customers.Core.Infrastructure.Exceptions
{
    internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
    {
        private static readonly ConcurrentDictionary<Type, string> Codes = new();

        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                CustomException ex => new ExceptionResponse(new { code = GetCode(ex), reason = ex.Message },
                    HttpStatusCode.BadRequest),
                _ => new ExceptionResponse(new { code = "error", reason = "There was an error." },
                    HttpStatusCode.BadRequest)
            };

        private static string GetCode(Exception exception)
            => Codes.GetOrAdd(exception.GetType(), exception.GetExceptionCode());
    }
}