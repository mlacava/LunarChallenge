using Common.Commands;
using Data.Models;

namespace Logic.Interfaces
{
    public interface IRocketsLogic
    {
        public Task<List<Rocket>> GetAllRocketsAsync(string sortBy);

        public Task<Rocket?> GetRocketAsync(string id);

        public Task CreateUpdateRocketAsync(MessageCommand messageCommand);

    }
}