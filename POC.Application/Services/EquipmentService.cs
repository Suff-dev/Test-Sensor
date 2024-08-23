using POC.Application.Dtos;
using POC.Application.Interfaces;
using POC.Domain.Entities;
using POC.Domain.Repositories;

namespace POC.Application.Services
{

    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;

        public EquipmentService(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        public async Task AddEquipmentAsync(AddEquipmentDto equipmentDto)
        {
            var equipment = new Equipment
            {
                Id = Guid.NewGuid().ToString(),
                Name = equipmentDto.Name,
            };

            await _equipmentRepository.AddAsync(equipment);
        }
        
        public async Task<List<EquipmentDto>> GetAllAsync()
        {
            var equipments = await _equipmentRepository.GetAllAsync();
            var equipmentDtos = equipments.Select(e => new EquipmentDto
            {
                Id = e.Id,
                Name = e.Name,
                SensorIds = e.SensorIds
            }).ToList();

            return equipmentDtos;
        }
    }
}
