using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Convey;
using Convey.CQRS.Events;
using Inflow.Services.Users.Core.Events;

namespace Inflow.Services.Users.Core.Services
{
    internal sealed class NotificationsRegistry : INotificationsRegistry
    {
        private static readonly ConcurrentDictionary<string, ConcurrentDictionary<string, bool>> Registrations = new();

        private static readonly HashSet<string> Names = new()
        {
            GetName<SignedUp>(), GetName<SignedIn>(), GetName<UserStateUpdated>()
        };

        private readonly IHttpClientFactory _clientFactory;

        public NotificationsRegistry(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task RegisterAsync(string name, string callbackUrl)
        {
            if (!Names.Contains(name))
            {
                throw new InvalidOperationException($"Invalid notification: '{name}'.");
            }

            var client = _clientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(callbackUrl, new Notification("setup", new { }));
            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Callback URL is unreachable: '{callbackUrl}'.");
            }

            Registrations.AddOrUpdate(name, _ => new ConcurrentDictionary<string, bool>()
            {
                [callbackUrl] = true
            }, (_, urls) =>
            {
                urls.TryAdd(callbackUrl, true);
                return urls;
            });
        }

        public async Task NotifyAsync<T>(T @event) where T : IEvent
        {
            var name = GetName<T>();
            if (!Registrations.TryGetValue(name, out var registrations))
            {
                return;
            }

            var client = _clientFactory.CreateClient();
            var notification = new Notification(name, @event);
            var tasks = registrations.Select(url => client.PostAsJsonAsync(url.Key, notification));
            await Task.WhenAll(tasks);
        }

        private record Notification(string Name, object Data);
        
        private static string GetName<T>() => typeof(T).Name.Underscore();
    }
}