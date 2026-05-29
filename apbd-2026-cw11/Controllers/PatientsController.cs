using apbd_2026_cw11.DTOs;
using apbd_2026_cw11.Exceptions;
using apbd_2026_cw11.Services;
using Microsoft.AspNetCore.Mvc;

namespace apbd_2026_cw11.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IDbService _dbService;
    public PatientsController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(string? search)
    {
        var res = await _dbService.GetPatients(search);
        return Ok(res);
    }

    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> AddBedAssignment(string pesel, [FromBody] AddBedAssignmentDto addBedAssignmentDto)
    {
        try
        {
            await _dbService.AddBedAssignmentAsync(pesel, addBedAssignmentDto);
            return Created();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
}