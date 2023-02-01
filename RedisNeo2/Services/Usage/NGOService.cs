using Neo4jClient;
using RedisNeo2.Models.Entities;
using RedisNeo2.Services.Implementation;

namespace RedisNeo2.Services.Usage
{

    public class NGOService : INGOService
    {
        
        private readonly IGraphClient client;

        public NGOService(IGraphClient client)
        {
            this.client = client;
        }

        public bool AddNGO(NGO noviNGO)
        {
            this.client.Cypher.Create("(n:NGO $noviNGO)")
                              .WithParam("noviNGO", noviNGO)
                              .ExecuteWithoutResultsAsync(); ;

            return true;
        }

        public bool Delete(int Pib)
        {
            var trazeni = this.client.Cypher.Match("(ngo: NGO)")
                                            .Where((NGO ngo) => ngo.Pib == Pib)
                                            .Return(ngo => ngo.As<NGO>())
                                            .ResultsAsync;
            if (trazeni == null)
                return false;

            this.client.Cypher.Match("(ngo: NGO)<-r-()")
                              .Where((NGO ngo) => ngo.Pib == Pib)
                              .Delete("ngo, d")
                              .ExecuteWithoutResultsAsync();


            return true;
        }

        public IEnumerable<NGO> FindByEmail(string email)
        {
            var trazeni = this.client.Cypher.Match("(ngo: NGO)")
                                            .Where((NGO ngo) => ngo.Email == email)
                                            .Return(ngo => ngo.As<NGO>())
                                            .ResultsAsync;

            return trazeni.Result;

        }

        public IEnumerable<NGO> GetAll()
        {
            var organizacije = this.client.Cypher
                            .Match("(x: NGO)")
                            .Return(x => x.As<NGO>())
                            .ResultsAsync;


            return organizacije.Result;
        }
    }
}
