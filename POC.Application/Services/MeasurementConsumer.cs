using MassTransit;
using POC.Application.Interfaces;
using POC.Domain.Entities;
using POC.Domain.Repositories;

namespace POC.Application.Services
{
    public class MeasurementConsumer : IConsumer<IMeasurementCreated>
    {
        private readonly IMeasurementRepository _measurementRepository;
        private readonly ISensorRepository _sensorRepository;

        public MeasurementConsumer(IMeasurementRepository measurementRepository, ISensorRepository sensorRepository)
        {
            _measurementRepository = measurementRepository;
            _sensorRepository = sensorRepository;
        }

        public async Task Consume(ConsumeContext<IMeasurementCreated> context)
        {
            var sensor = await _sensorRepository.GetByIdAsync(context.Message.SensorId);
            if (sensor == null)
            {
                throw new Exception("Sensor not found");
            }

            var measurement = new Measurement
            {
                Id = context.Message.Id,
                Created = context.Message.Created,
                Value = context.Message.Value,
                Unit = context.Message.Unit
            };

            await _measurementRepository.AddAsync(measurement, context.Message.SensorId);
        }
    }
}
