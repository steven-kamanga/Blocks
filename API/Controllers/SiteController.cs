using System.Security.Claims;
using API.Models.Dto.Blocks;
using API.Models.Dto.Site;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SiteController : ControllerBase
{
    private readonly ISiteService _siteService;

    public SiteController(ISiteService siteService)
    {
        _siteService = siteService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateSite([FromBody] SiteCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Location))
        {
            return BadRequest(new { Message = "Name and Location cannot be empty!" });
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if(userId == null) return Unauthorized(new { Message = "User not authenticated!" });
        var site = await _siteService.CreateSiteAsync(request, userId);
        return Ok(site);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSites([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var sites = await _siteService.GetAllSitesAsync(page, pageSize);
        return Ok(sites);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var site = await _siteService.GetByIdAsync(id);
        if (site == null)
            return NotFound(new { Message = "Site not found." });
        
        return Ok(site);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] SiteUpdateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Location))
        {
            return BadRequest(new { Message = "Name and Location cannot be empty!" });
        }

        var success = await _siteService.UpdateAsync(id, request);
        if (!success)
            return NotFound(new { Message = "Site not found or name already exists." });
        
        return Ok(new { Message = "Site updated successfully." });
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _siteService.DeleteAsync(id);
        if (!success)
            return NotFound(new { Message = "Site not found." });
        
        return Ok(new { Message = "Site deleted successfully." });
    }
}