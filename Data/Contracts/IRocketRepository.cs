using Data.Models;

namespace Data.Contracts
{
    public interface IRocketRepository
    {
        public Task<List<Rocket>> GetAsync();

        public Task<Rocket?> GetAsync(string id);

        public Task CreateAsync(Rocket newRocket);

        public Task UpdateAsync(string id, Rocket updatedRocket);

    }
}