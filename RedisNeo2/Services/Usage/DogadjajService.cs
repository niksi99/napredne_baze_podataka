using Neo4jClient;
using Neo4jClient.Cypher;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using System.Collections;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using RedisNeo2.Services.Usage;
using StackExchange.Redis;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace RedisNeo2.Services.Usage
{
    public class DogadjajService : IDogadjajService
    {
        private readonly IGraphClient client;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly INGOService ngoservice;
        public DogadjajService(INGOService ngoservice, IGraphClient client, IHttpContextAccessor httpContextAccessor)
        {
            this.client = client;
            this.ngoservice = ngoservice;
            this.httpContextAccessor = httpContextAccessor;


        }

       

        public bool AddEvent(Dogadjaj noviDogadjaj)
        {
            
            //NGO myNGO = (NGO)this.ngoservice.FindByEmail(email);
            var novi = new Dogadjaj
            {
                Naziv = noviDogadjaj.Naziv,
                BrojUcesnika = noviDogadjaj.BrojUcesnika,
                TrenutniBrojUcesnika = noviDogadjaj.TrenutniBrojUcesnika,
                Opis = noviDogadjaj.Opis,
                Lokacija = noviDogadjaj.Lokacija,
                Organizator = noviDogadjaj.Organizator
            };
            this.client.Cypher.Create("(d:Dogadjaj $noviDogadjaj)")
                             .WithParam("noviDogadjaj", noviDogadjaj)
                             .ExecuteWithoutResultsAsync(); ;

            var b = this.client.Cypher.Match("(d: Dogadjaj)")
                .Where((Dogadjaj d) =>
                d.Naziv == noviDogadjaj.Naziv)
                .Return(d => d.As<Dogadjaj>())
                .ResultsAsync;

            var a = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (b.Result != null) {
                this.client.Cypher
                            .Match("(d:Dogadjaj), (ngo:NGO)")
                            // .Match("(k)-[r:PrijavljenNa]->(d)")
                            .Where((Dogadjaj d, NGO ngo) =>
                                 d.Naziv == noviDogadjaj.Naziv &&
                                 ngo.Email == a)
                            .Create("(ngo)-[r:Organizuje]->(d)")
                            //.Return(d => d.As<Dogadjaj>()).ResultsAsync;
                            .ExecuteWithoutResultsAsync();
            }
            return true;

//            MATCH(myNGO: NGO)
//WHERE myNGO.Email = "tikva@gmail.com"
//CREATE(myNGO) -[r: Organizuje]->(d: Dogadjaj {
//            Naziv: "Ekologija",
//    Opis: "NFDJQENDJQ",
//    BrojUcesnika: 56,
//    TrenutniBrojUcesnika: 34
   
//})
           
            
        }
        private string ulogovani() => httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

        public bool Delete(string Naziv)
        {
            

            this.client.Cypher.OptionalMatch("(d:Dogadjaj)<-[r]-()")
                              .Where((Dogadjaj d) => d.Naziv == Naziv)
                              .Delete("r, d")
                              .ExecuteWithoutResultsAsync();


            return true;
        }

        public IEnumerable<Dogadjaj> GetAll()
        {
            var dogadjaji = this.client.Cypher
                            .Match("(x: Dogadjaj)")
                            .Return(x => x.As<Dogadjaj>())
                            .ResultsAsync;

           
            return dogadjaji.Result;
            
        }

        

        public Dogadjaj FindByName()
        {
            throw new NotImplementedException();
        }

        public bool PrijaviSe(string dog, string NGOName, string KorisnikEmail)
        {
            var tajdog = this.client.Cypher
                            .Match("(d:Dogadjaj)")
                            .Where((Dogadjaj d) => d.Naziv == dog)
                            .Return(d => d.As<Dogadjaj>()).ResultsAsync;

           

          
            if (tajdog == null)
                return false;

            var tajngo = this.client.Cypher
                            .Match("(ngo:NGO)")
                            .Where((NGO ngo) => ngo.UserName == NGOName)
                            .Return(ngo => ngo.As<NGO>()).ResultsAsync;

            Dogadjaj mojD = new Dogadjaj();
            mojD.Naziv = dog;
            if (tajngo == null)
                return false;


            var tajkorisnik = this.client.Cypher
                                      .Match("(kor:Korisnik)")
                                      .Where((Korisnik kor) => kor.Email == KorisnikEmail)
                                      .Return(kor => kor.As<Korisnik>());


            if (tajkorisnik == null)
                return false;

            Korisnik mojK = new Korisnik();
            mojK.Email = KorisnikEmail;
            NGO mojaNgo = new NGO();
            mojaNgo.UserName = NGOName;
            if (mojD.Organizator == NGOName && mojD.TrenutniBrojUcesnika < mojD.BrojUcesnika)
            {
                mojD.PrijavljeniKorisnici.Add(mojK);
                mojD.TrenutniBrojUcesnika++;
            }


            //this.client.Cypher
            //           .Match("(d:Dogadjaj)", "(k:Korisnik)")
            //           .Where((Dogadjaj d, Korisnik k) =>
            //                d.Naziv == dog &&
            //                k.Email == KorisnikEmail &&
            //                d.Organizator == NGOName)
            //           .Create("(k)-[r:PrijavljenNa]->(d)")
            //           .ExecuteWithoutResultsAsync();
            //await this.client.Cypher.Match("(d:Department), (e:Employee)")
            //                .Where((Dogadjaj d, Korisnik e) => d.Naziv == dog && e.Email == KorisnikEmail)
            //                .Create("(k)-[r:PrijavljenNa]->(d)")
            //                .ExecuteWithoutResultsAsync();
            //.Create("(d)-[r:hasEmployee]->(e)")
            return true;
            //this.client.Cypher();
        }
    }
}
