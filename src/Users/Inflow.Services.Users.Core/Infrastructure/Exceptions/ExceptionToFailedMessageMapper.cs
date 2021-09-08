using System;
using Convey.MessageBrokers.RabbitMQ;
using Inflow.Services.Users.Core.Events;
using Inflow.Services.Users.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Users.Core.Infrastructure.Exceptions
{
    internal sealed class ExceptionToFailedMessageMapper : IExceptionToFailedMessageMapper
    {
        public FailedMessage Map(Exception exception, object message)
            => exception switch
            {
                CustomException ex => new FailedMessage(new UserActionRejected(ex.Message, ex.GetExceptionCode()), false),
                DbUpdateException => new FailedMessage(false),
                _ => new FailedMessage(new UserActionRejected("There was an error", "users_service_error")),
            };
    }
}

