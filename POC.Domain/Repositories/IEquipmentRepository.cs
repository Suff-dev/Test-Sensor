using POC.Domain.Entities;

namespace POC.Domain.Repositories
{
    public interface IEquipmentRepository
    {
        Task AddAsync(Equipment equipment);
        Task<Equipment> GetByIdAsync(string id);
        Task<List<Equipment>> GetAllAsync();
    }

}
