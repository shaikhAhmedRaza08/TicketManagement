using TicketManagement.Domain;

namespace TicketManagement.Application.Dtos;

public class CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
}

public class ProjectResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int TicketCount { get; set; }
    public DateTime CreatedAt { get; set; }

    public static ProjectResponse From(Project p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Key = p.Key,
        TicketCount = p.Tickets?.Count ?? 0,
        CreatedAt = p.CreatedAt
    };
}
