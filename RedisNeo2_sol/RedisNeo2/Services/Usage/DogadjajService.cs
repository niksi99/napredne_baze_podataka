using Neo4jClient;
using Neo4jClient.Cypher;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;
using System.Collections;

namespace RedisNeo2.Services.Usage
{
    public class DogadjajService : IDogadjajService
    {
        private readonly IGraphClient client;

        public DogadjajService(IGraphClient client)
        {
            this.client = client;
        }

       

        public bool AddEvent(Dogadjaj noviDogadjaj)
        {
            
                this.client.Cypher.Create("(d:Dogadjaj $noviDogadjaj)")
                             .WithParam("noviDogadjaj", noviDogadjaj)
                             .ExecuteWithoutResultsAsync(); 

                return true;
           
            
        }

        public bool Delete(string Naziv)
        {
            var trazeni = this.client.Cypher.Match("(d: Dogadjaj)")
                                            .Where((Dogadjaj d) => d.Naziv == Naziv)
                                            .Return(d => d.As<Dogadjaj>())
                                            .ResultsAsync;
            if (trazeni == null)
                return false;

            this.client.Cypher.Match("(d: Dogadjaj)<-r-()")
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

        
    }
}
