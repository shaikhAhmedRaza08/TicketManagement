using TicketManagement.Domain;

namespace TicketManagement.Application.Dtos;

public class CreateTicketRequest
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public TicketType Type { get; set; } = TicketType.Task;
    public string Reporter { get; set; } = string.Empty;
    public string? Assignee { get; set; }
}

public class UpdateTicketRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; }
    public TicketType Type { get; set; }
    public string? Assignee { get; set; }
}

public class ChangeStatusRequest
{
    public TicketStatus NewStatus { get; set; }
}

/// <summary>Query/filter parameters for the ticket list (bound from the query string).</summary>
public class TicketQueryParameters
{
    public Guid? ProjectId { get; set; }
    public TicketStatus? Status { get; set; }
    public TicketPriority? Priority { get; set; }
    public string? Assignee { get; set; }
    public string? Search { get; set; }   // matches title/description

    private int _page = 1;
    public int Page { get => _page; set => _page = value < 1 ? 1 : value; }

    private int _pageSize = 20;
    public int PageSize { get => _pageSize; set => _pageSize = value is < 1 or > 100 ? 20 : value; }
}

/// <summary>Lightweight ticket shape for list views.</summary>
public class TicketListItemResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Assignee { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static TicketListItemResponse From(Ticket t) => new()
    {
        Id = t.Id,
        Title = t.Title,
        Status = t.Status.ToString(),
        Priority = t.Priority.ToString(),
        Type = t.Type.ToString(),
        Assignee = t.Assignee,
        ProjectId = t.ProjectId,
        CreatedAt = t.CreatedAt,
        UpdatedAt = t.UpdatedAt
    };
}

/// <summary>Full ticket shape including description and comments.</summary>
public class TicketResponse
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Reporter { get; set; } = string.Empty;
    public string? Assignee { get; set; }
    public List<CommentResponse> Comments { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static TicketResponse From(Ticket t) => new()
    {
        Id = t.Id,
        ProjectId = t.ProjectId,
        Title = t.Title,
        Description = t.Description,
        Status = t.Status.ToString(),
        Priority = t.Priority.ToString(),
        Type = t.Type.ToString(),
        Reporter = t.Reporter,
        Assignee = t.Assignee,
        Comments = (t.Comments ?? new())
            .OrderBy(c => c.CreatedAt)
            .Select(CommentResponse.From)
            .ToList(),
        CreatedAt = t.CreatedAt,
        UpdatedAt = t.UpdatedAt
    };
}

/// <summary>Generic paged result wrapper.</summary>
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = new List<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
