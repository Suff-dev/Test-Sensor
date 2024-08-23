using Microsoft.AspNetCore.Mvc;
using POC.Application.Dtos;
using POC.Application.Interfaces;

namespace POCAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly ISensorService _sensorService;

        public SensorController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSensorsByEquipmentId([FromQuery] string equipmentId)
        {
            var sensors = await _sensorService.GetSensorsByEquipmentIdAsync(equipmentId);
            return Ok(sensors);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddSensorDto sensorDto)
        {

            await _sensorService.AddSensorAsync(sensorDto);
            return Ok(sensorDto);
        }
    }
}
