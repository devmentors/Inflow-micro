using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Inflow.Services.Customers.Core.Clients.DTO;

namespace Inflow.Services.Customers.Core.Clients;

internal sealed class UserApiClient : IUserApiClient
{
    private const string Url = "http://localhost:5030";
    private readonly IHttpClientFactory _clientFactory;

    public UserApiClient(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public Task<UserDto> GetAsync(string email)
    {
        var client = _clientFactory.CreateClient();
        return client.GetFromJsonAsync<UserDto>($"{Url}/users/by-email/{email}");
    }
}