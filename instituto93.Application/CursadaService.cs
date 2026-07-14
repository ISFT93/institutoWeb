using System.Collections.Generic;
using System.Threading.Tasks;
using instituto93.Domain.Models;
using instituto93.Application.Interfaces;
using instituto93.Data.Interfaces;

namespace instituto93.Application.Services
{
    public class CursadaService : ICursadaService
    {
        private readonly ICursadaRepository _repository;
        public CursadaService(ICursadaRepository repository) => _repository = repository;

        public async Task<IEnumerable<Cursada>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Cursada> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddAsync(Cursada model) => await _repository.AddAsync(model);
        public async Task UpdateAsync(Cursada model) => await _repository.UpdateAsync(model);
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
    }
}