using System.Security.Claims;
using API.Models.Dto.Batch;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/batch")]
public class BatchController(IBatchService batchService) : ControllerBase
{
    private readonly IBatchService _batchService = batchService;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBatch([FromBody] BatchCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.BatchName))
        {
            return BadRequest(new BatchCreateResponse
            {
                Message = "Batch name cannot be empty!"
            });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId == null) return Unauthorized(new BatchCreateResponse
        {
            Message = "User not authenticated!"
        });
        var result = await _batchService.CreateBatchAsync(request, userId);
        
        if (result.Message == "This batch name already exists.")
            return Conflict(result);
        
        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllBatches([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var batches = await _batchService.GetAllBatchesAsync(page, pageSize);
        return Ok(batches);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var batch = await _batchService.GetByIdAsync(id);
        if (batch == null)
            return NotFound(new { Message = "Batch not found." });
        
        return Ok(batch);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] BatchUpdateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.BatchName))
        {
            return BadRequest(new { Message = "Batch name cannot be empty!" });
        }

        var success = await _batchService.UpdateAsync(id, request);
        if (!success)
            return NotFound(new { Message = "Batch not found or name already exists." });
        
        return Ok(new { Message = "Batch updated successfully." });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _batchService.DeleteAsync(id);
        if (!success)
            return NotFound(new { Message = "Batch not found." });
        
        return Ok(new { Message = "Batch deleted successfully." });
    }

    [HttpPost("calculate")]
    [Authorize]
    public async Task<IActionResult> CalculateOutput([FromBody] BatchCalculationRequest request)
    {
        var result = await _batchService.CalculateOutputAsync(request);
        return Ok(result);
    }
}