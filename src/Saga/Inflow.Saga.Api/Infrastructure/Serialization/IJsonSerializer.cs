namespace Inflow.Saga.Api.Infrastructure.Serialization
{
    internal interface IJsonSerializer
    {
        string Serialize<T>(T value);
        T Deserialize<T>(string value);
    }
}