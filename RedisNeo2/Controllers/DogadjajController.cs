using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using RedisNeo2.Models.DTOs;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;

namespace RedisNeo2.Controllers
{
    public class DogadjajController : Controller
    {
        private readonly ILogger<DogadjajController> _logger;
        private readonly IGraphClient _client;
        private readonly IDogadjajService dogadjaj_service;
        private readonly IKorisnikService korisnik_service;

        public DogadjajController(IKorisnikService korisnik_service, IDogadjajService dogadjaj_service, IGraphClient client, ILogger<DogadjajController> logger)
        {
            _client = client;
            _logger = logger;
            this.dogadjaj_service = dogadjaj_service;
            this.korisnik_service = korisnik_service;
        }

        public IActionResult Dogadjaj()
        {
            return View();
        }

        
        [Authorize]
        [HttpPost]
        public IActionResult AddEvent(Dogadjaj model) {

            var addEventResultSuccess = this.dogadjaj_service.AddEvent(model);

            if (addEventResultSuccess) {
                TempData["msg"] = "Added Successfully";
                return RedirectToAction("GetAll", "Dogadjaj");
                //return RedirectToAction(nameof(AddEvent));
            }

            TempData["msg"] = "Error has occured on server side";
            //return RedirectToPage("/Dogadjaj/GetAll");

            return View(TempData);
        }

        //[HttpPost]
        //public IActionResult PrijaviSe(string dog, string NGOName, string KorisnikName) {
        //    this.dogadjaj_service.PrijaviSe(dog, NGOName, KorisnikName);
           
        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> PrijaviSe(PrijaviSeDTO p)
        {
            //var a = 98;
            var b = await this._client.Cypher
                .OptionalMatch("(korisnik:Korisnik)-[r:PrijavljenNa]->(dogadjaj:Dogadjaj)")
                .Where((Korisnik korisnik, Dogadjaj dogadjaj) =>
                korisnik.Email == p.EmailKorisnika &&
                dogadjaj.Naziv == p.ImeDogadjaja)
                .Return(korisnik => korisnik.As<Korisnik>())
                .ResultsAsync;

            if (b.First() == null) {
                await this._client.Cypher
                            .Match("(d:Dogadjaj), (k:Korisnik)")
                            // .Match("(k)-[r:PrijavljenNa]->(d)")
                            .Where((Dogadjaj d, Korisnik k) =>
                                 d.Naziv == p.ImeDogadjaja &&
                                 k.Email == p.EmailKorisnika &&
                                 d.Organizator == p.Organizator)
                            
                            .Create("(k)-[r:PrijavljenNa]->(d)")
                            //.Return(d => d.As<Dogadjaj>()).ResultsAsync;
                            .ExecuteWithoutResultsAsync();

                return RedirectToAction("UspesnaPrijava", "Dogadjaj");
            }

            
            return RedirectToAction("NeuspesnaPrijava", "Dogadjaj");
           // return RedirectToAction("/Dogadjaj/GetAll");
        }

        public IActionResult PrijaviSePage()
        {
            return View();
        }

        public IActionResult UspesnaPrijava() {
            return View();
        }

        public IActionResult NeuspesnaPrijava()
        {
            return View();
        }

        public IActionResult GetAll() {

            var podaci = this.dogadjaj_service.GetAll();
            return View(podaci);
        }

        public IActionResult AddEventPage()
        {

            return View();
        }

        public IActionResult Prijavi() {
            return View();
        }
      
        //[HttpGet]
        //public async Task<IActionResult> GetAll() {

        //    var dogadjaji = await _client.Cypher
        //                    .Match("(x: Dogadjaj)")
        //                     .Return(x => x.As<Dogadjaj>()).ResultsAsync;
        //    return Ok(dogadjaji);
        //}

    }
}
