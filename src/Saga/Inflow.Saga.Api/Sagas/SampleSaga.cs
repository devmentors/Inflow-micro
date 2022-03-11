using System;
using System.Threading.Tasks;
using Chronicle;
using Inflow.Saga.Api.Messages;
using Inflow.Saga.Api.Services;

namespace Inflow.Saga.Api.Sagas;

internal sealed class SampleSaga : Saga<SampleSagaData>,
    ISagaStartAction<SignedUp>,
    ISagaAction<SignedIn>
{
    private readonly IMessageBroker _messageBroker;

    public SampleSaga(IMessageBroker messageBroker)
    {
        _messageBroker = messageBroker;
    }

    public override SagaId ResolveId(object message, ISagaContext context)
        => message switch
        {
            SignedUp m => m.UserId.ToString(),
            SignedIn m => m.UserId.ToString(),
            _ => base.ResolveId(message, context)
        };

    public Task HandleAsync(SignedUp message, ISagaContext context)
    {
        Data.CreatedAt = DateTime.UtcNow;
        Data.Id = "123";
        return Task.CompletedTask;
    }

    public Task CompensateAsync(SignedUp message, ISagaContext context) => Task.CompletedTask;

    public Task HandleAsync(SignedIn message, ISagaContext context)
    {
        var id = Data.Id;
        return Task.CompletedTask;
    }

    public Task CompensateAsync(SignedIn message, ISagaContext context) => Task.CompletedTask;
}

internal sealed class SampleSagaData
{
    public DateTime CreatedAt { get; set; }
    public string Id { get; set; }
}