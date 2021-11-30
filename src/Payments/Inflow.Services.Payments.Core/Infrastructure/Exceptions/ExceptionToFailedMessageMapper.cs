using System;
using Convey.MessageBrokers.RabbitMQ;
using Inflow.Services.Payments.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Payments.Core.Infrastructure.Exceptions;

internal sealed class ExceptionToFailedMessageMapper : IExceptionToFailedMessageMapper
{
    public FailedMessage Map(Exception exception, object message)
        => exception switch
        {
            CustomException ex => new FailedMessage(new PaymentsActionRejected(ex.Message, ex.GetExceptionCode()), false),
            DbUpdateException => new FailedMessage(false),
            _ => new FailedMessage(new PaymentsActionRejected("There was an error", "payments_service_error")),
        };
}