
using Microsoft.AspNetCore.Mvc;
using RedisNeo2.Hubs;
using RedisNeo2.Models.DTOs;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text.Json;

namespace RedisNeo2.Services.Usage
{
    public class ChatService : IChatServices
    {
        private readonly IConnectionMultiplexer _cmux;
        private readonly IHttpContextAccessor httpContextAccessor;
        //private readonly IDatabase _redisDb;
        private readonly string Channel = "Kanal";
        public ChatService(IConnectionMultiplexer cmux, IHttpContextAccessor httpContextAccessor)
        {
            _cmux = cmux;
            this.httpContextAccessor = httpContextAccessor;
            //_redisDb = redis.GetDatabase();
        }

        public string GetMessage()
        {
            throw new NotImplementedException();
        }

        public async Task SendMessage(string message)
        {
            var user = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var subscriber = _cmux.GetSubscriber();
            string A = string.Concat(user, "^");
            string B = string.Concat(A, message);

            await subscriber.PublishAsync(Channel, B);
        }

        public async Task<PorukaDTO> Receive() {

            PorukaDTO vratiPoruku = new PorukaDTO();

            var subscriber = _cmux.GetSubscriber();
     
            subscriber.SubscribeAsync(Channel, (channel, porukaPLUScovek) => {
                string[] subs = porukaPLUScovek.ToString().Split("^");
                vratiPoruku.korisnik = subs[0];
                vratiPoruku.poruka = subs[1];
            });
            Task.Delay(1000);
            return vratiPoruku;
        }
    }
}
