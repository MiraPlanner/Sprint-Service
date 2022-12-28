using Microsoft.AspNetCore.Mvc;
using Sprint_Service.Dtos;
using Sprint_Service.Interfaces;
using Sprint_Service.Models;

namespace Sprint_Service.Controllers;

[ApiController]
[Route("sprints")]
public class SprintController : ControllerBase
{
    private readonly ISprintService _sprintService;

    public SprintController(ISprintService sprintService)
    {
        _sprintService = sprintService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SprintDto>>> GetAll()
    {
        var sprints = await _sprintService.GetAll();

        return Ok(sprints);
    }
    
    [HttpGet("backlog")]
    public async Task<ActionResult<IEnumerable<Issue>>> GetBacklog()
    {
        var issues = await _sprintService.GetBacklog();

        return Ok(issues);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SprintDto>> GetById(Guid id)
    {
        var sprint = await _sprintService.GetById(id);

        if (sprint == null) return NotFound();
        
        return Ok(sprint);
    }

    [HttpPost]
    public async Task<ActionResult<SprintDto>> Create(CreateSprintDto createSprintDto)
    {
        var sprint = await _sprintService.Create(createSprintDto);
        
        return CreatedAtAction(nameof(GetById), new { id = sprint.Id }, sprint);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateSprintDto updateSprintDto)
    {
        var sprint = await _sprintService.Update(id, updateSprintDto);

        if (sprint == null) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var sprint = await _sprintService.Delete(id);

        if (sprint == null) return NotFound();

        return NoContent();
    }
}