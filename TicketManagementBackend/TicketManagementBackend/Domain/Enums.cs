namespace TicketManagement.Domain;

public enum TicketStatus
{
    Open,
    InProgress,
    Resolved,
    Closed
}

public enum TicketPriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum TicketType
{
    Bug,
    Feature,
    Task
}
