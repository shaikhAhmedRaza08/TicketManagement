using TicketManagement.Domain;

namespace TicketManagement.Application.Dtos;

public class CreateCommentRequest
{
    public string Author { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public class CommentResponse
{
    public Guid Id { get; set; }
    public string Author { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public static CommentResponse From(TicketComment c) => new()
    {
        Id = c.Id,
        Author = c.Author,
        Body = c.Body,
        CreatedAt = c.CreatedAt
    };
}
