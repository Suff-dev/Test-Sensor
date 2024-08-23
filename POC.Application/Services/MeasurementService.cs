using MassTransit;
using POC.Application.Dtos;
using POC.Application.Interfaces;
using POC.Domain.Entities;
using POC.Domain.Repositories;

namespace POC.Application.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementRepository _measurementRepository;
        private readonly ISensorRepository _sensorRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public MeasurementService(IMeasurementRepository measurementRepository, ISensorRepository sensorRepository, IPublishEndpoint publishEndpoint)
        {
            _measurementRepository = measurementRepository;
            _sensorRepository = sensorRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task AddMeasurementAsync(MeasurementDto measurementDto, string sensorId)
        {
            var sensor = await _sensorRepository.GetByIdAsync(sensorId);
            if (sensor == null)
            {
                throw new Exception("Sensor not found");
            }

            var measurement = new Measurement
            {
                Id = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow,
                Value = measurementDto.Value,
                Unit = measurementDto.Unit
            };

            await _publishEndpoint.Publish<IMeasurementCreated>(new
            {
                measurement.Id,
                SensorId = sensorId,
                measurement.Value,
                measurement.Created,
                measurement.Unit
            });
        }
    }
}
