using System.Collections.Generic;
using System.Threading.Tasks;
using instituto93.Domain.Models;
using instituto93.Data.Interfaces;


namespace instituto93.Data.Repositories
{
    public class CursadaRepository : ICursadaRepository
    {
        

        public async Task<IEnumerable<Cursada>> GetAllAsync()
        {
          
            return new List<Cursada>();
        }

        public async Task<Cursada> GetByIdAsync(int id)
        {
           
            return null;
        }

        public async Task AddAsync(Cursada model)
        {
           
        }

        public async Task UpdateAsync(Cursada model)
        {
           
        }

        public async Task DeleteAsync(int id)
        {
         
        }
    }
}