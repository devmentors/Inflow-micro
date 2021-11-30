namespace Inflow.APIGateway.Serialization;

internal interface IJsonSerializer
{
    string Serialize<T>(T value);
    T Deserialize<T>(string value);
}