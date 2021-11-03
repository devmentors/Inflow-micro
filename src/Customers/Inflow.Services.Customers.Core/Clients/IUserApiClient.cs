﻿using System.Threading.Tasks;
using Inflow.Services.Customers.Core.Clients.DTO;

namespace Inflow.Services.Customers.Core.Clients
{
    internal interface IUserApiClient
    {
        Task<UserDto> GetUserAsync(string email);
        Task SubscribeNotificationAsync(string name, string callbackUrl);
    }
}