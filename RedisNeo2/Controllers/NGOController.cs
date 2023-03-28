using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using RedisNeo2.Models;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using System.Security.Claims;

namespace RedisNeo2.Controllers
{
   
    public class NGOController : Controller
    {
        private readonly ILogger<NGOController> _logger;
        private readonly INGOService service;
        private readonly IGraphClient client;

        public NGOController(INGOService service, IGraphClient client, ILogger<NGOController> logger)
        {
            this.service = service;
            this.client = client;
            _logger = logger;
        }

        public IActionResult NGO()
        {
            return View();
        }

        
        public IActionResult LoginNGOPage()
        {
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(string email, string lozinka) {

            var claims = new List<Claim> {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, "NGO")
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

            return RedirectToAction("LoggedInNGO", "NGO");
        }

        [HttpPost]
        public async Task<IActionResult> Logout() {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AddNGOPage");
        }

        [HttpPost]
        public IActionResult Add(NGO model) {

            var ngo = this.service.AddNGO(model);
            if (ngo) {
                return RedirectToAction("AddNGOPage", "NGO");
            }
            return View();
        }

        public IActionResult AddNGOPage() {
            return View();
        }

        public IActionResult GetAll() {

            var sveOrganizacije = this.service.GetAll();
            return View(sveOrganizacije);
        }

     

        [Authorize]
        public IActionResult LoggedInNGO()
        {
            return View();
        }

    }
}
