using POC.Application.Dtos;

namespace POC.Application.Interfaces
{
    public interface ISensorService
    {
        Task<List<SensorDto>> GetSensorsByEquipmentIdAsync(string equipmentId);
        Task AddSensorAsync(AddSensorDto sensorDto);
    }
}
