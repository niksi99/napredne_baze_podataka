using RedisNeo2.Models.Entities;

namespace RedisNeo2.Services.Implementation
{
    public interface IChatServices
    {
        public Task SendMessage(string user, string message);
        public Task GetMessage();

        public IEnumerable<Message> GetAll();
    }
}
