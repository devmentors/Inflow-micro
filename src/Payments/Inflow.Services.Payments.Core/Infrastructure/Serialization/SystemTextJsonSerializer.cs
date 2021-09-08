using System.Text.Json;
using System.Text.Json.Serialization;

namespace Inflow.Services.Payments.Core.Infrastructure.Serialization
{
    internal sealed class SystemTextJsonSerializer : IJsonSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {new JsonStringEnumConverter()}
        };

        public string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

        public T Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, Options);
    }
}