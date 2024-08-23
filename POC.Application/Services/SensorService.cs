using POC.Application.Dtos;
using POC.Application.Interfaces;
using POC.Domain.Entities;
using POC.Domain.Repositories;

namespace POC.Application.Services
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly IEquipmentRepository _equipmentRepository;

        public SensorService(ISensorRepository sensorRepository, IEquipmentRepository equipmentRepository)
        {
            _sensorRepository = sensorRepository;
            _equipmentRepository = equipmentRepository;
        }

        public async Task<List<SensorDto>> GetSensorsByEquipmentIdAsync(string equipmentId)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(equipmentId);
            if (equipment == null)
            {
                throw new Exception("Equipment not found");
            }

            // Recupera os sensores associados ao equipamento
            var sensors = await _sensorRepository.GetByEquipmentIdAsync(equipmentId);
            var sensorDtos = sensors.Select(sensor => new SensorDto
            {
                Id = sensor.Id,
                Name = sensor.Name,
                EquipmentId = sensor.EquipmentId,
                Measurements = sensor.Measurements
                    .OrderBy(m => m.Created)
                    .Take(10)
                    .Select(m => new MeasurementDto
                    {
                        Value = m.Value,
                        Unit = m.Unit                        
                    }).ToList()
            }).ToList();

            return sensorDtos;
        }

        public async Task AddSensorAsync(AddSensorDto sensorDto)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(sensorDto.EquipmentId);
            if (equipment == null)
            {
                throw new Exception("Equipment not found");
            }

            var sensor = new Sensor
            {
                Id = Guid.NewGuid().ToString(),
                Name = sensorDto.Name,
                EquipmentId = sensorDto.EquipmentId,
                Measurements = new List<Measurement>()
            };

            await _sensorRepository.AddAsync(sensor);
        }
    }
}
