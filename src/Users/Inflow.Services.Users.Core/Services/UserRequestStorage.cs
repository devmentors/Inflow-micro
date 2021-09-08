using System;
using Inflow.Services.Users.Core.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace Inflow.Services.Users.Core.Services
{
    internal sealed class UserRequestStorage : IUserRequestStorage
    {
        private readonly IMemoryCache _cache;

        public UserRequestStorage(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void SetToken(Guid commandId, JwtDto jwt)
            => _cache.Set(GetKey(commandId), jwt, TimeSpan.FromSeconds(5));

        public JwtDto GetToken(Guid commandId)
            => _cache.Get<JwtDto>(GetKey(commandId));

        private static string GetKey(Guid commandId) => $"jwt:{commandId:N}";
    }
}