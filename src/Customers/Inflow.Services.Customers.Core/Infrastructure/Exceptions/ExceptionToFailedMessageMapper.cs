using System;
using Convey.MessageBrokers.RabbitMQ;
using Inflow.Services.Customers.Core.Events;
using Inflow.Services.Customers.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Inflow.Services.Customers.Core.Infrastructure.Exceptions;

internal sealed class ExceptionToFailedMessageMapper : IExceptionToFailedMessageMapper
{
    public FailedMessage Map(Exception exception, object message) => null;
}