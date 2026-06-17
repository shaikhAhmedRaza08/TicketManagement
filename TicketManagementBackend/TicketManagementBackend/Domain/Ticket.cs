namespace TicketManagement.Domain;

/// <summary>A single issue/ticket: a bug, feature, or task tracked through a status workflow.</summary>
public class Ticket
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public TicketType Type { get; set; } = TicketType.Task;

    public string Reporter { get; set; } = string.Empty;
    public string? Assignee { get; set; }

    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }

    public List<TicketComment> Comments { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
