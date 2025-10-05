using Data.DataContext;
using Data.Models;
using Data.Contracts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Data.Repositories
{
    public class RocketRepository : IRocketRepository
    {
        private readonly IMongoCollection<Rocket> _rocketsCollection;

        public RocketRepository(IOptions<RocketStoreDatabaseSettings> rocketStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                rocketStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                rocketStoreDatabaseSettings.Value.DatabaseName);

            _rocketsCollection = mongoDatabase.GetCollection<Rocket>(
                rocketStoreDatabaseSettings.Value.CollectionName);
        }

        public async Task<List<Rocket>> GetAsync() =>
            await _rocketsCollection.Find(_ => true).ToListAsync();

        public async Task<Rocket?> GetAsync(string id) =>
            await _rocketsCollection.Find(x => x.Channel == Guid.Parse(id)).FirstOrDefaultAsync();

        public async Task CreateAsync(Rocket newRocket)
        {
            await _rocketsCollection.InsertOneAsync(newRocket);
        }

        public async Task UpdateAsync(string id, Rocket updatedRocket) =>
            await _rocketsCollection.ReplaceOneAsync(x => x.Channel == Guid.Parse(id), updatedRocket);

    }
}
