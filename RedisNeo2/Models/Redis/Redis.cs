using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisNeo2.Models.Redis
{
    public static class Redis
    {
        private static readonly ConnectionMultiplexer redis;
        public static IDatabase Database { get; set; }
        public static ISubscriber Subscriber { get; set; }

        static Redis() {
            redis = ConnectionMultiplexer.Connect(new ConfigurationOptions { EndPoints = { "localhost:6379" } });
            Database = redis.GetDatabase();
            Subscriber = redis.GetSubscriber();
        }
    }

    public static class RedisManager<T> {

        private static T Deserialize(RedisValue redisData) => JsonConvert.DeserializeObject<T>(redisData);
        private static string Serialize(T redisData) => JsonConvert.SerializeObject(redisData);

        public static void Push(string path, T redisData)
            => Redis.Database.ListRightPush(path, Serialize(redisData));

        public static T Pop(string path)
           => Deserialize(Redis.Database.ListRightPop(path));

        public static void Subscribe(string path, Action<string, T> onPub)
            => Redis.Subscriber.Subscribe(path, (x, y) => onPub(x, Deserialize(y)));

        public static void Publish(string path, T redisData)
            => Redis.Database.Publish(path, Serialize(redisData));
    }
}
