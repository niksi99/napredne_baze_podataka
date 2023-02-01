using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using RedisNeo2.Services.Usage;
using System.Security.Claims;
using System.Text.Json;

namespace RedisNeo2.Hubs
{
    public class ChatHub : Hub
    {
        private readonly INGOService _ngoService;
        private readonly IKorisnikService _korService;
        private readonly IChatServices _chatService;

        public ChatHub(INGOService ngoService, IKorisnikService k, IChatServices chatService)
        {
            _ngoService = ngoService;
            _chatService = chatService;
            _korService = k;
        }

        public async Task SendMessage(string user, string messageString)
        {
            //var message = JsonSerializer.Deserialize<Message>(messageString);

            if (messageString != null)
            {
                await _chatService.SendMessage(user, messageString);
            }
        }

        public async Task GetMessage() {
            //await _chatService.GetMessage();    
        }

        public async Task SendMessage1(string user, string message) {
            await Clients.All.SendAsync("aaaa", user, message);
        }

        //public async Task JoinRoom(RoomConnection rc) {
        //    await Groups.AddToGroupAsync(Context.ConnectionId, rc.Room);
        //    await Clients.Group(rc.Room).SendAsync("chatapp", _userBot, $"{rc.User} has joined {rc.Room}");
        //}

        //public async Task SendMessage(string user, string message) =>
        //    await Clients.All.SendAsync("chatapp", user, message);

        //public string GetConnectionID() => Context.ConnectionId;

        public string re() => Context.User.FindFirstValue(ClaimTypes.Email);

        public string ren() => Context.User.FindFirstValue(ClaimTypes.Email);
        public string rek() => Context.User.FindFirstValue(ClaimTypes.Email);
        //public async Task SendToUser(string user, string receiverConnectonID, string message) =>
        //    await Clients.Client(receiverConnectonID).SendAsync("chatapp", user, message);

        //[Authorize(Roles = "Korisnik")]
        //public async Task KDopisivanje(string user, string receiverConnectonID, string message) =>
        //   await Clients.Client(receiverConnectonID).SendAsync("chatapp", user, message);
        //private static Dictionary<string, string> Users = new Dictionary<string, string>();

        //public override async Task OnConnectedAsync()
        //{
        //    string username = Context.GetHttpContext().Request.Query["username"];
        //    Users.Add(Context.ConnectionId, username);
        //    await AddMessageToChat(string.Empty, $"{username} joined the party!");
        //    await base.OnConnectedAsync();
        //}

        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    string username = Users.FirstOrDefault(u => u.Key == Context.ConnectionId).Value;
        //    await AddMessageToChat(string.Empty, $"{username} left!");
        //}

        //public async Task AddMessageToChat(string user, string message)
        //{
        //    await Clients.All.SendAsync("GetThatMessageDude", user, message);
        //}
    }
}
