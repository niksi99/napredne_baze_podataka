using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;

namespace RedisNeo2.Controllers
{
    public class DogadjajController : Controller
    {
        private readonly ILogger<DogadjajController> _logger;
        private readonly IGraphClient _client;
        private readonly IDogadjajService dogadjaj_service;

        public DogadjajController(IDogadjajService dogadjaj_service, IGraphClient client, ILogger<DogadjajController> logger)
        {
            _client = client;
            _logger = logger;
            this.dogadjaj_service = dogadjaj_service;
        }

        public IActionResult Dogadjaj()
        {
            return View();
        }

        //[Route("AddEvent")]
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

        

        public IActionResult GetAll() {

            var podaci = this.dogadjaj_service.GetAll();
            return View(podaci);
        }

        public IActionResult AddEventPage()
        {

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
