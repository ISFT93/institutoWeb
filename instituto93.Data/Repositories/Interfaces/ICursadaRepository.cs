using System.Collections.Generic;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Data.Interfaces
{
    public interface ICursadaRepository
    {
        Task<IEnumerable<Cursada>> GetAllAsync();
        Task<Cursada> GetByIdAsync(int id);
        Task AddAsync(Cursada model);
        Task UpdateAsync(Cursada model);
        Task DeleteAsync(int id);
    }
}