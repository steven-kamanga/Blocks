using API.Models.Dto.RawMaterial;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/materials")]
public class RawMaterialController(IRawMaterialService materialService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] RawMaterialCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new RawMaterialCreateResponse
            {
                Message = "Material name cannot be empty!"
            });
        }

        var result = await materialService.CreateAsync(request);
        
        if (result.Message.Contains("already exists"))
            return Conflict(result);
        
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var materials = await materialService.GetAllAsync(page, pageSize);
        return Ok(materials);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var material = await materialService.GetByIdAsync(id);
        if (material == null)
            return NotFound(new { Message = "Material not found." });
        
        return Ok(material);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] RawMaterialUpdateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new { Message = "Material name cannot be empty!" });
        }

        var success = await materialService.UpdateAsync(id, request);
        if (!success)
            return NotFound(new { Message = "Material not found or name already exists." });
        
        return Ok(new { Message = "Material updated successfully." });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await materialService.DeleteAsync(id);
        if (!success)
            return NotFound(new { Message = "Material not found." });
        
        return Ok(new { Message = "Material deleted successfully." });
    }

    [HttpPatch("{id}/stock")]
    [Authorize]
    public async Task<IActionResult> UpdateStock(int id, [FromBody] decimal quantity)
    {
        var success = await materialService.UpdateStockAsync(id, quantity);
        if (!success)
            return NotFound(new { Message = "Material not found." });
        
        return Ok(new { Message = "Stock updated successfully." });
    }
}
