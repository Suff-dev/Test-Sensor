using POC.Domain.Entities;

namespace POC.Domain.Repositories
{
    public interface ISensorRepository
    {
        Task AddAsync(Sensor sensor);
        Task<Sensor> GetByIdAsync(string id);
        Task<List<Sensor>> GetByEquipmentIdAsync(string equipmentId);
        Task<List<Sensor>> GetAllAsync();
    }
}
