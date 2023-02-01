
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

            
            //var jsonMessage = JsonSerializer.Serialize<Message>(message);
            var A = new { 
                messageId,
                roomKey,
                user,
                message
            };
            await _redisDb.SortedSetAddAsync(roomKey, JsonSerializer.Serialize<Message>(m), DateTime.Now.ToOADate());
            await PublishMessage(m);
        }

        public string  GetMessage() {
            var sortedSetData = _redisDb.SortedSetScan("Room:1");
            var vrati = (string.Join("\n", sortedSetData));

            return vrati;
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

        private async Task SubsMessage(Message pubSubMessage)
        {
           //await _redisD ("MESSAGES", pubSubMessage);
        }
    }
}
