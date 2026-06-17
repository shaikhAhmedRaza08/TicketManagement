using TicketManagement.Application.Dtos;
using TicketManagement.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace TicketManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _service;
    public TicketsController(ITicketService service) => _service = service;

    /// <summary>Create a ticket under a project.</summary>
    [HttpPost]
    public async Task<ActionResult<TicketResponse>> Create([FromBody] CreateTicketRequest request)
    {
        var (ok, error, result) = await _service.CreateAsync(request);
        return ok
            ? CreatedAtAction(nameof(GetById), new { id = result!.Id }, result)
            : BadRequest(new { error });
    }

    /// <summary>Get a single ticket with its comments.</summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TicketResponse>> GetById(Guid id)
    {
        var result = await _service.GetAsync(id);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>List tickets with optional filters (project, status, priority, assignee, search) and pagination.</summary>
    [HttpGet]
    public async Task<ActionResult<PagedResult<TicketListItemResponse>>> Query([FromQuery] TicketQueryParameters query)
        => Ok(await _service.QueryAsync(query));

    /// <summary>Update a ticket's editable fields.</summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TicketResponse>> Update(Guid id, [FromBody] UpdateTicketRequest request)
    {
        var (ok, error, result) = await _service.UpdateAsync(id, request);
        return ok ? Ok(result) : (error == "Ticket not found." ? NotFound(new { error }) : BadRequest(new { error }));
    }

    /// <summary>Change a ticket's status (validated against the workflow rules).</summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<TicketResponse>> ChangeStatus(Guid id, [FromBody] ChangeStatusRequest request)
    {
        var (ok, error, result) = await _service.ChangeStatusAsync(id, request);
        return ok ? Ok(result) : (error == "Ticket not found." ? NotFound(new { error }) : BadRequest(new { error }));
    }

    /// <summary>Add a comment to a ticket.</summary>
    [HttpPost("{id:guid}/comments")]
    public async Task<ActionResult<CommentResponse>> AddComment(Guid id, [FromBody] CreateCommentRequest request)
    {
        var (ok, error, result) = await _service.AddCommentAsync(id, request);
        return ok ? Ok(result) : (error == "Ticket not found." ? NotFound(new { error }) : BadRequest(new { error }));
    }
}
