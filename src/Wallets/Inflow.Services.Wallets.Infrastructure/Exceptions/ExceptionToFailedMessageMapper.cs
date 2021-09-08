using System;
using Convey.MessageBrokers.RabbitMQ;
using Inflow.Services.Wallets.Application.Exceptions;
using Inflow.Services.Wallets.Core.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Wallets.Infrastructure.Exceptions
{
    internal sealed class ExceptionToFailedMessageMapper : IExceptionToFailedMessageMapper
    {
        public FailedMessage Map(Exception exception, object message)
            => exception switch
            {
                AppException ex => new FailedMessage(new WalletsActionRejected(ex.Message, ex.GetExceptionCode()), false),
                DomainException ex => new FailedMessage(new WalletsActionRejected(ex.Message, ex.GetExceptionCode()), false),
                DbUpdateException => new FailedMessage(false),
                _ => new FailedMessage(new WalletsActionRejected("There was an error", "wallets_service_error")),
            };
    }
}

