using Microsoft.AspNetCore.Mvc;

namespace RedisNeo2.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult LoginKorisnik()
        {
            return View();
        }

        public IActionResult LoginNGO()
        {
            return View();
        }
    }
}
