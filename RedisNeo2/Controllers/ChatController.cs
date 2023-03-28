using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RedisNeo2.Models.Entities;
using System.Security.Claims;

namespace RedisNeo2.Controllers
{
    public class ChatController : Controller
    {
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





    }
}
