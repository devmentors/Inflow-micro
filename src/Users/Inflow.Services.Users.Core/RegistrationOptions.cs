using System.Collections.Generic;

namespace Inflow.Services.Users.Core
{
    internal class RegistrationOptions
    {
        public bool Enabled { get; set; }
        public IEnumerable<string> InvalidEmailProviders { get; set; }
    }
}