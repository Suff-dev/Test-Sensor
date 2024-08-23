using Microsoft.AspNetCore.Mvc;
using POC.Application.Dtos;
using POC.Application.Interfaces;

namespace POCAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeasurementController : ControllerBase
    {
        private readonly IMeasurementService _measurementService;

        public MeasurementController(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MeasurementDto measurementDto, [FromQuery] string sensorId)
        {
            try
            {
                await _measurementService.AddMeasurementAsync(measurementDto, sensorId);
                return Ok(measurementDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
    }
}
