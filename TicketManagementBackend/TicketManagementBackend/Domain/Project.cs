namespace TicketManagement.Domain;

/// <summary>A container that groups related tickets (like a board in Jira).</summary>
public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;   // short code, e.g. "PAY"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Ticket> Tickets { get; set; } = new();
}
