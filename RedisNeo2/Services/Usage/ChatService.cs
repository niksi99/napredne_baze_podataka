
using Microsoft.AspNetCore.Mvc;
using RedisNeo2.Hubs;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisNeo2.Services.Usage
{
    public class ChatService : IChatServices
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _redisDb;

        public ChatService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _redisDb = redis.GetDatabase();
        }

        public async Task SendMessage(string user, string message)
        {
            var messageId = Guid.NewGuid().ToString();
            //message.Id = messageId;

            var roomKey = "room:1";

            Message m = new Message();

            m.Id = messageId;
            m.UserName = user;
            m.VremeSlanja = DateTime.Now;
            m.MessageText = message;

    
            await _redisDb.SortedSetAddAsync(roomKey, JsonSerializer.Serialize<Message>(m), DateTime.Now.ToOADate());
            await PublishMessage(m);
        }

        public async Task GetMessage() {
            var sortedSetData = _redisDb.SortedSetScan("room:1");
            
            var vrati = (string.Join("\n", sortedSetData));

            //return vrati;

            Message[] niz = new Message[5];
            Message m = new Message();
            niz[0] = m;

            m.Id = "xx";
            m.UserName = "yy";
            m.VremeSlanja = DateTime.Now;
            m.MessageText = "zzz";

            await PublishMessages(niz);
        }
        //private async Task PublishMessage(string type, Message data)
        //{
        //    var jsonData = JsonSerializer.Serialize<Message>(data);

        //    var pubSubMessage = new PubSub()
        //    {
        //        Type = type,
        //        Data = jsonData
        //    };

        //    await PublishMessage(pubSubMessage.ToString();
        //}

        private async Task PublishMessage(Message pubSubMessage)
        {
            await _redisDb.PublishAsync("MESSAGES", JsonSerializer.Serialize<Message>(pubSubMessage));
        }

        private async Task PublishMessages(Message[] pubSubMessage)
        {
            await _redisDb.PublishAsync("MESSAGES", JsonSerializer.Serialize<Message[]>(pubSubMessage));
        }

        public IEnumerable<Message> GetAll()
        {
            //var dogadjaji = this.client.Cypher
            //                .Match("(x: Dogadjaj)")
            //                .Return(x => x.As<Dogadjaj>())
            //                .ResultsAsync;

            var sortedSetData = _redisDb.SortedSetScan("room:1");

            var vrati = (string.Join("\n", sortedSetData));
            string[] poruke_splitovane = vrati.Split("/n");
            Message[] p = new Message[10000];
            foreach (var i in poruke_splitovane) {
                string[] P = i.Split(" ");
            }
            List<Message> niz = new List<Message>();
            Message m0 = new Message();
            Message m1 = new Message();
            

            m0.Id = "xx";
            m0.UserName = "yy";
            m0.VremeSlanja = DateTime.Now;
            m0.MessageText = "zzz";

            m1.Id = "fwef";
            m1.UserName = "Mikiii";
            m1.VremeSlanja = DateTime.Now;
            m1.MessageText = "AAAAA";
            niz.Add(m0);
            niz.Add(m1);

            IEnumerable<Message> A = niz;
            return A;
        }
    }
}
