using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Convey.HTTP;
using Inflow.Services.Customers.Core.Clients.DTO;

namespace Inflow.Services.Customers.Core.Clients
{
    internal sealed class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _client;
        private readonly string _url;

        public UserApiClient(HttpClient client, HttpClientOptions options)
        {
            _client = client;
            _url = options.Services["users"];
        }

        public async Task<UserDto> GetUserAsync(string email)
        {
            var response = await _client.GetAsync($"{_url}/users/by-email/{email}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task SubscribeNotificationAsync(string name, string callbackUrl)
        {
            var response = await _client.PostAsJsonAsync($"{_url}/notifications/subscribe", new { name, callbackUrl});
            if (!response.IsSuccessStatusCode)
            {
                return;
            }
        }
    }
}