using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using RedisNeo2.Models;
using RedisNeo2.Models.DTOs;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using System.Security.Claims;

namespace RedisNeo2.Controllers
{
   
    public class NGOController : Controller
    {
        private readonly ILogger<NGOController> _logger;
        private readonly INGOService service;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IGraphClient client;

        public NGOController(INGOService service, IGraphClient client, ILogger<NGOController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
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

        public IActionResult PostojiNGO() {
            return View();
        }

        [Authorize]
        public IActionResult LoggedInNGO()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "NGO")]
        public async Task<IActionResult> UpdateN(UpdateKorisnikNgoDTO k)
        {
            var a = this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var result = await client.Cypher.Match("(nn:NGO)")
                                            .Where((NGO nn) => nn.Email == a)
                                            .Return(nn => nn.As<NGO>()).ResultsAsync;

            NGO staraNgo = result.First();
            staraNgo.Lozinka = k.NovaLozinka;

            await this.client.Cypher
            .Match("(n1:NGO)")
            .Where((NGO n1) => n1.Email == a)
            .Set("n1 = $ngoo")
            .WithParam("ngoo", staraNgo)
            .ExecuteWithoutResultsAsync();

            return NoContent();
        }

        public IActionResult UpdateNgo()
        {

            return View();
        }

    }
}
