namespace Inflow.Services.Users.Core.Infrastructure.Serialization
{
    internal interface IJsonSerializer
    {
        string Serialize<T>(T value);
        T Deserialize<T>(string value);
    }
}