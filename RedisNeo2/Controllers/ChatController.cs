using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using System.Security.Claims;

namespace RedisNeo2.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatServices service;

        public ChatController(IChatServices service) {
            this.service = service;
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
            var podaci = this.service.GetAll();
            return View(podaci);
        }

        [Authorize(Roles = "NGO")]
        public IActionResult ChatVjuNGO()
        {
            return View();
        }

        public IActionResult GetAll() {
            var podaci = this.service.GetAll();
            return View(podaci);
        }





    }
}
