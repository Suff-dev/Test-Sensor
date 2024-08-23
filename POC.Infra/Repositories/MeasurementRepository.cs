using MongoDB.Driver;
using POC.Domain.Entities;
using POC.Domain.Repositories;
using POC.Infra.Persistence;

namespace POC.Infra.Repositories
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly IMongoCollection<Sensor> _sensors;

        public MeasurementRepository(MongoDbContext context)
        {
            _sensors = context.Sensors;
        }

        public async Task AddAsync(Measurement measurement, string sensorId)
        {
            var filter = Builders<Sensor>.Filter.Eq(s => s.Id, sensorId);
            var update = Builders<Sensor>.Update.Push(s => s.Measurements, measurement);
            await _sensors.UpdateOneAsync(filter, update);
        }

        public async Task<List<Measurement>> GetLatestMeasurementsAsync(string sensorId, int limit)
        {
            var sensor = await _sensors.Find(s => s.Id == sensorId).FirstOrDefaultAsync();
            return sensor?.Measurements.OrderByDescending(m => m.Created).Take(limit).ToList();
        }
    }

}
