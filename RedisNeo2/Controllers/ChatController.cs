using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RedisNeo2.Hubs;
using RedisNeo2.Models.DTOs;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using System.Security.Claims;

namespace RedisNeo2.Controllers
{
    public class ChatController : Controller
    {
        public readonly IChatServices chatService;

        public ChatController(IChatServices chatService) {
            this.chatService = chatService;
        }

        [Authorize]
        public IActionResult ChatPage()
        {
            //var currentLogedInUser = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //ViewBag.CurrentLogedIn = currentLogedInUser;
            return View();
        }

        [Authorize(Roles = "Korisnik")]
        public IActionResult ChatVjuKorisnik() {
            return View();
        }

        [Authorize(Roles = "NGO")]
        public IActionResult ChatVjuNGO()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Send(string poruka) {

            await this.chatService.SendMessage(poruka);

            return NoContent();
        }

        [HttpGet]
        public async Task<PorukaDTO> Receive() {

            var D = await this.chatService.Receive();
            ViewBag.Covek = D.korisnik;
            ViewBag.Poruka = D.poruka;
            return D;
        }

    }
}
