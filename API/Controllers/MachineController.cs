using API.Models.Dto.Machine;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/machines")]
public class MachineController(IMachineService machineService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] MachineCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new MachineCreateResponse
            {
                Message = "Machine name cannot be empty!"
            });
        }

        if (request.SiteId <= 0)
        {
            return BadRequest(new MachineCreateResponse
            {
                Message = "A valid site must be selected!"
            });
        }

        var result = await machineService.CreateAsync(request);
        
        if (result.Message.Contains("already exists"))
            return Conflict(result);
        
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var machines = await machineService.GetAllAsync(page, pageSize);
        return Ok(machines);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var machine = await machineService.GetByIdAsync(id);
        if (machine == null)
            return NotFound(new { Message = "Machine not found." });
        
        return Ok(machine);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] MachineUpdateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { Message = "Machine name cannot be empty!" });
        }

        if (request.SiteId <= 0)
        {
            return BadRequest(new { Message = "A valid site must be selected!" });
        }

        var success = await machineService.UpdateAsync(id, request);
        if (!success)
            return NotFound(new { Message = "Machine not found or name already exists." });
        
        return Ok(new { Message = "Machine updated successfully." });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await machineService.DeleteAsync(id);
        if (!success)
            return NotFound(new { Message = "Machine not found." });
        
        return Ok(new { Message = "Machine deleted successfully." });
    }

    [HttpPatch("{id}/toggle")]
    [Authorize]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var success = await machineService.ToggleActiveAsync(id);
        if (!success)
            return NotFound(new { Message = "Machine not found." });
        
        return Ok(new { Message = "Machine status toggled successfully." });
    }
}
