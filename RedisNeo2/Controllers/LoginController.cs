using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RedisNeo2.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult LoginPage()
        {
            return View();
        }
        public IActionResult LoginKorisnik()
        {
            return View();
        }

        public IActionResult LoginNGO()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login_kao, string email, string lozinka)
        {

            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Email, email),
                    //new Claim(ClaimTypes.Name, login_kao),
                    new Claim(ClaimTypes.Role, login_kao)
                };

            ClaimsIdentity identitycl = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            ClaimsPrincipal principalcl = new ClaimsPrincipal(identitycl);

            var properties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principalcl,
                properties
            );

            if (login_kao.Equals("Korisnik"))
               //return RedirectToAction("LoggedInKorisnik", "Korisnik");
               return RedirectToAction("LoggedInKorisnik", "Korisnik");
            else if (login_kao.Equals("NGO"))
                return RedirectToAction("LoggedInNGO", "NGO");
            else
                return RedirectToAction("LoginPage", "Login");
        }
    }
}
