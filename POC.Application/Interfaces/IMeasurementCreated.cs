
namespace POC.Application.Interfaces
{
    public interface IMeasurementCreated
    {
        string Id { get; }
        string SensorId { get; }
        double Value { get; }
        string Unit { get; set; }
        DateTime Created { get; }
    }
}