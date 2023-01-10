using RedisNeo2.Models.Entities;

namespace RedisNeo2.Services.Implementation
{
    public interface INGOService
    {
        public IEnumerable<NGO> GetAll();
        public bool AddNGO(NGO noviNGO);
        public bool Delete(int Pib);
        public IEnumerable<NGO> FindByEmail(string email);
        //public bool LoginNGO(string email, string lozinka);
    }
}
