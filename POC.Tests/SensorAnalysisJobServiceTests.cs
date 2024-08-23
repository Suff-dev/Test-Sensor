using Moq;
using POC.Application.Interfaces;
using POC.Application.Services;
using POC.Domain.Entities;
using POC.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace POC.Tests
{
    public class SensorAnalysisJobServiceTests
    {
        private readonly Mock<ISensorRepository> _sensorRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly SensorAnalysisJobService _sensorAnalysisJobService;

        public SensorAnalysisJobServiceTests()
        {
            _sensorRepositoryMock = new Mock<ISensorRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            _sensorAnalysisJobService = new SensorAnalysisJobService(_sensorRepositoryMock.Object, _emailServiceMock.Object, null);
        }

        [Fact]
        public async Task AnalyzeSensors_ShouldSendEmail_WhenConditionsMet()
        {
            // Arrange
            var sensor = new Sensor
            {
                Id = "sensor1",
                Name = "Temperature Sensor",
                Measurements = new List<Measurement>
        {
            new Measurement { Value = 20, Created = DateTime.UtcNow.AddMinutes(-5) },
            new Measurement { Value = 20.5, Created = DateTime.UtcNow.AddMinutes(-4) },
            new Measurement { Value = 21, Created = DateTime.UtcNow.AddMinutes(-3) },
            new Measurement { Value = 21.5, Created = DateTime.UtcNow.AddMinutes(-2) },
            new Measurement { Value = 22, Created = DateTime.UtcNow.AddMinutes(-1) },
        }
            };

            // A média será 21 e a margem de erro de 5% será 1.05 (então todas as medições devem estar dentro do intervalo [19.95, 22.05])
            _sensorRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Sensor> { sensor });

            // Act
            await _sensorAnalysisJobService.AnalyzeSensors();

            // Assert
            _emailServiceMock.Verify(
                service => service.SendEmailAsync(
                    "recipient@example.com",
                    "Sensor Alert",
                    It.Is<string>(s => s.Contains(sensor.Name))),
                Times.Once);
        }

        [Fact]
        public async Task AnalyzeSensors_ShouldNotSendEmail_WhenConditionsAreNotMet()
        {
            // Arrange
            var sensor = new Sensor
            {
                Id = "sensor1",
                Name = "Temperature Sensor",
                Measurements = new List<Measurement>
                {
                    new Measurement { Value = 20, Created = DateTime.UtcNow.AddMinutes(-5) },
                    new Measurement { Value = 19, Created = DateTime.UtcNow.AddMinutes(-4) },
                    new Measurement { Value = 18, Created = DateTime.UtcNow.AddMinutes(-3) },
                    new Measurement { Value = 17, Created = DateTime.UtcNow.AddMinutes(-2) },
                    new Measurement { Value = 16, Created = DateTime.UtcNow.AddMinutes(-1) },
                }
            };

            _sensorRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Sensor> { sensor });

            // Act
            await _sensorAnalysisJobService.AnalyzeSensors();

            // Assert
            _emailServiceMock.Verify(
                service => service.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }
    }
}
