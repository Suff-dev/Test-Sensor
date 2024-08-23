using MongoDB.Driver;
using POC.Domain.Entities;
using POC.Domain.Repositories;
using POC.Infra.Persistence;

namespace POC.Infra.Repositories
{
    public class SensorRepository : ISensorRepository
    {
        private readonly IMongoCollection<Sensor> _sensors;

        public SensorRepository(MongoDbContext context)
        {
            _sensors = context.Sensors;
        }

        public async Task AddAsync(Sensor sensor)
        {
            await _sensors.InsertOneAsync(sensor);
        }

        public async Task<Sensor> GetByIdAsync(string id)
        {
            return await _sensors.Find(s => s.Id == id).FirstOrDefaultAsync();
        }
        public async Task<List<Sensor>> GetByEquipmentIdAsync(string equipmentId)
        {
            return await _sensors.Find(s => s.EquipmentId == equipmentId).ToListAsync();
        }

        public async Task<List<Sensor>> GetAllAsync()
        {
            return await _sensors.Find(_ => true).ToListAsync();
        }
    }
}
