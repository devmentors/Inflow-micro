using System;
using Convey.MessageBrokers.RabbitMQ;

namespace Inflow.Services.Customers.Core.Infrastructure.Exceptions;

internal sealed class ExceptionToFailedMessageMapper : IExceptionToFailedMessageMapper
{
    public FailedMessage Map(Exception exception, object message) => null;
}