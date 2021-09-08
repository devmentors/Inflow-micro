using System.Threading.Tasks;
using Convey.HTTP;
using Inflow.Services.Customers.Core.Clients.DTO;

namespace Inflow.Services.Customers.Core.Clients
{
    internal sealed class UserApiClient : IUserApiClient
    {
        private readonly IHttpClient _client;
        private readonly string _url;

        public UserApiClient(IHttpClient client, HttpClientOptions options)
        {
            _client = client;
            _url = options.Services["users"];
        }

        public Task<UserDto> GetAsync(string email)
            => _client.GetAsync<UserDto>($"{_url}/users/by-email/{email}");
    }
}