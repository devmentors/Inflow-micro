using System;
using System.Collections.Generic;

namespace Inflow.Services.Users.Core.DTO
{
    public class JwtDto
    {
        public string AccessToken { get; set; }
        public long Expiry { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public IDictionary<string, IEnumerable<string>> Claims { get; set; }
    }
}