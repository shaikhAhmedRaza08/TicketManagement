using TicketManagement.Application.Dtos;
using TicketManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace TicketManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;
    public ProjectsController(IProjectService service) => _service = service;

    /// <summary>Create a new project.</summary>
    [HttpPost]
    public async Task<ActionResult<ProjectResponse>> Create([FromBody] CreateProjectRequest request)
    {
        var (ok, error, result) = await _service.CreateAsync(request);
        return ok
            ? CreatedAtAction(nameof(GetById), new { id = result!.Id }, result)
            : BadRequest(new { error });
    }

    /// <summary>Get a project by id (includes ticket count).</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProjectResponse>> GetById(Guid id)
    {
        var result = await _service.GetAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>List all projects.</summary>
    [HttpGet]
    public async Task<ActionResult<List<ProjectResponse>>> GetAll()
        => Ok(await _service.GetAllAsync());
}
