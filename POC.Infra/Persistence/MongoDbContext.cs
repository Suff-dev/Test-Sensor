using Microsoft.Extensions.Options;
using MongoDB.Driver;
using POC.Domain.Entities;
using POC.Domain.Settings;

namespace POC.Infra.Persistence
{

    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Equipment> Equipments => _database.GetCollection<Equipment>(nameof(Equipments));
        public IMongoCollection<Sensor> Sensors => _database.GetCollection<Sensor>(nameof(Sensors));
        public IMongoCollection<Measurement> Measurements => _database.GetCollection<Measurement>(nameof(Measurements));
    }

}
