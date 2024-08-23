using Microsoft.AspNetCore.Mvc;
using POC.Application.Dtos;
using POC.Application.Interfaces;

namespace POCAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipmentController : ControllerBase
    {
        private readonly IEquipmentService _equipmentService;

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddEquipmentDto equipmentDto)
        {
            await _equipmentService.AddEquipmentAsync(equipmentDto);
            return Ok(equipmentDto);

        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _equipmentService.GetAllAsync());
        }
    }
}
