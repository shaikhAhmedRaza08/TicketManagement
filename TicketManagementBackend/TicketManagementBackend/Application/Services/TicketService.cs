using TicketManagement.Application.Dtos;
using TicketManagement.Application.Workflow;
using TicketManagement.Domain;
using TicketManagement.Infrastructure.Data;

namespace TicketManagement.Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _tickets;
    private readonly IProjectRepository _projects;
    private readonly TicketStatusWorkflow _workflow;

    public TicketService(ITicketRepository tickets, IProjectRepository projects, TicketStatusWorkflow workflow)
    {
        _tickets = tickets;
        _projects = projects;
        _workflow = workflow;
    }

    public async Task<(bool ok, string? error, TicketResponse? result)> CreateAsync(CreateTicketRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return (false, "Title is required.", null);

        var project = await _projects.GetByIdAsync(request.ProjectId);
        if (project is null)
            return (false, "Project not found.", null);

        var ticket = new Ticket
        {
            ProjectId = request.ProjectId,
            Title = request.Title.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            Priority = request.Priority,
            Type = request.Type,
            Reporter = request.Reporter.Trim(),
            Assignee = string.IsNullOrWhiteSpace(request.Assignee) ? null : request.Assignee.Trim(),
            Status = TicketStatus.Open
        };

        await _tickets.AddAsync(ticket);
        return (true, null, TicketResponse.From(ticket));
    }

    public async Task<TicketResponse?> GetAsync(Guid id)
    {
        var ticket = await _tickets.GetByIdAsync(id);
        return ticket is null ? null : TicketResponse.From(ticket);
    }

    public async Task<PagedResult<TicketListItemResponse>> QueryAsync(TicketQueryParameters query)
    {
        var (items, total) = await _tickets.QueryAsync(query);
        return new PagedResult<TicketListItemResponse>
        {
            Items = items.Select(TicketListItemResponse.From).ToList(),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = total
        };
    }

    public async Task<(bool ok, string? error, TicketResponse? result)> UpdateAsync(Guid id, UpdateTicketRequest request)
    {
        var ticket = await _tickets.GetByIdAsync(id);
        if (ticket is null)
            return (false, "Ticket not found.", null);

        if (string.IsNullOrWhiteSpace(request.Title))
            return (false, "Title is required.", null);

        ticket.Title = request.Title.Trim();
        ticket.Description = request.Description?.Trim() ?? string.Empty;
        ticket.Priority = request.Priority;
        ticket.Type = request.Type;
        ticket.Assignee = string.IsNullOrWhiteSpace(request.Assignee) ? null : request.Assignee.Trim();
        ticket.UpdatedAt = DateTime.UtcNow;

        await _tickets.SaveChangesAsync();
        return (true, null, TicketResponse.From(ticket));
    }

    public async Task<(bool ok, string? error, TicketResponse? result)> ChangeStatusAsync(Guid id, ChangeStatusRequest request)
    {
        var ticket = await _tickets.GetByIdAsync(id);
        if (ticket is null)
            return (false, "Ticket not found.", null);

        // Guard the transition through the workflow rules.
        if (!_workflow.CanTransition(ticket.Status, request.NewStatus))
        {
            var allowed = string.Join(", ", _workflow.AllowedNextStatuses(ticket.Status));
            return (false,
                $"Cannot move a ticket from {ticket.Status} to {request.NewStatus}. Allowed: {allowed}.",
                null);
        }

        ticket.Status = request.NewStatus;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _tickets.SaveChangesAsync();
        return (true, null, TicketResponse.From(ticket));
    }

    public async Task<(bool ok, string? error, CommentResponse? result)> AddCommentAsync(Guid ticketId, CreateCommentRequest request)
    {
        var ticket = await _tickets.GetByIdAsync(ticketId);
        if (ticket is null)
            return (false, "Ticket not found.", null);

        if (string.IsNullOrWhiteSpace(request.Body) || string.IsNullOrWhiteSpace(request.Author))
            return (false, "Author and Body are required.", null);

        var comment = new TicketComment
        {
            TicketId = ticketId,
            Author = request.Author.Trim(),
            Body = request.Body.Trim()
        };
        ticket.Comments.Add(comment);
        ticket.UpdatedAt = DateTime.UtcNow;

        await _tickets.SaveChangesAsync();
        return (true, null, CommentResponse.From(comment));
    }
}
