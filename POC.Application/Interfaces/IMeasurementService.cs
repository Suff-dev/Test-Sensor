using POC.Application.Dtos;

namespace POC.Application.Interfaces
{
    public interface IMeasurementService
    {
        Task AddMeasurementAsync(MeasurementDto measurementDto, string sensorId);

    }
}
