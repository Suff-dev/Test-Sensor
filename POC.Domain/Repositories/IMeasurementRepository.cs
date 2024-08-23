using POC.Domain.Entities;

namespace POC.Domain.Repositories
{
    public interface IMeasurementRepository
    {
        Task AddAsync(Measurement measurement, string sensorId);
        Task<List<Measurement>> GetLatestMeasurementsAsync(string sensorId, int limit);
    }
}
