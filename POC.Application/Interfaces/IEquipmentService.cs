using POC.Application.Dtos;

namespace POC.Application.Interfaces
{
    public interface IEquipmentService
    {
        Task AddEquipmentAsync(AddEquipmentDto equipment);
        Task<List<EquipmentDto>> GetAllAsync();
    }
}
