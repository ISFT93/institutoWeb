using System.Collections.Generic;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces
{
    public interface ICursadaService
    {
        Task<IEnumerable<Cursada>> GetAllAsync();
        Task<Cursada> GetByIdAsync(int id);
        Task AddAsync(Cursada model);
        Task UpdateAsync(Cursada model);
        Task DeleteAsync(int id);
    }
}