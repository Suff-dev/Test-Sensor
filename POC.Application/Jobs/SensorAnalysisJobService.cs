using Hangfire;
using POC.Application.Interfaces;
using POC.Domain.Entities;
using POC.Domain.Repositories;

namespace POC.Application.Services
{
    public class SensorAnalysisJobService
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly IEmailService _emailService;
        private readonly IRecurringJobManager _recurringJobManager;

        public SensorAnalysisJobService(ISensorRepository sensorRepository, IEmailService emailService, IRecurringJobManager recurringJobManager)
        {
            _sensorRepository = sensorRepository;
            _emailService = emailService;
            _recurringJobManager = recurringJobManager;
        }

        public void ScheduleSensorAnalysisJob()
        {
        }

        public async Task AnalyzeSensors()
        {
            var sensors = await _sensorRepository.GetAllAsync();

            foreach (var sensor in sensors)
            {
                var last50Measurements = sensor.Measurements.OrderByDescending(m => m.Created).Take(50).ToList();
                if (last50Measurements.Count >= 5)
                {
                    var last5Measurements = last50Measurements.Take(5).ToList();
                    var average = last50Measurements.Average(m => m.Value);

                    if (IsWithinErrorMargin(last5Measurements, average))
                    {
                        await _emailService.SendEmailAsync("recipient@example.com", "Sensor Alert", "A situação foi detectada no sensor: " + sensor.Name);
                    }
                }
            }
        }

        // Lógica para "Problemática" simulada
        private bool IsWithinErrorMargin(List<Measurement> last5Measurements, double average, double marginOfErrorPercentage = 5.0)
        {
            double marginOfError = average * (marginOfErrorPercentage / 100.0);

            foreach (var measurement in last5Measurements)
            {
                if (Math.Abs(measurement.Value - average) > marginOfError)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
