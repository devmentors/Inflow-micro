using System;
using Convey.MessageBrokers.RabbitMQ;
using Inflow.Services.Customers.Core.Events;
using Inflow.Services.Customers.Core.Events.External;
using Inflow.Services.Customers.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Customers.Core.Infrastructure.Exceptions;

internal sealed class ExceptionToFailedMessageMapper : IExceptionToFailedMessageMapper
{
    public FailedMessage Map(Exception exception, object message)
        => exception switch
        {
            CustomException ex => new FailedMessage(new CustomerActionRejected(ex.GetExceptionCode(), ex.Message), false),
            DbUpdateException => new FailedMessage(false),
            _ => new FailedMessage(new CustomerActionRejected("There was an error", "customers_service_error")),
        };
}