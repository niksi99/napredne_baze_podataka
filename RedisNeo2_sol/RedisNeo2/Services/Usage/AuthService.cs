using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Neo4jClient;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using System.Security.Claims;

namespace RedisNeo2.Services.Usage
{
    public class AuthService : IAuthService
    {
        private readonly IGraphClient client;

        public AuthService(IGraphClient client)
        {
            this.client = client;
        }

        private void DodajToSpisak(NGO n, Dogadjaj d) {
            n.SpisakDogadjaja.Add(d);
        }

        public bool Add(string organiz, string dog, Dogadjaj noviDogadjaj)
        {

            var D = noviDogadjaj;
            D.Organizator = organiz;
            this.client.Cypher.Match("org:NGO").Where((NGO org) => org.Naziv == organiz)
                              .Create("(org)-[r:Organizuje]->(d:Dogadjaj {D})")
                              .WithParam("D", D)
                              .ExecuteWithoutResultsAsync(); ;


            //this.client.Cypher.Match("(o:NGO), (d:Dogadjaj)")
            //                  .Where((NGO o, Dogadjaj d) => o.Naziv == organiz && d.Naziv == noviDogadjaj.Naziv)
            //                  .Create("o-[:Organizuje]->d")
            //                  .ExecuteWithoutResultsAsync();
            return true;

        }

        
    }
}

