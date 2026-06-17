using TicketManagement.Domain;

namespace TicketManagement.Application.Workflow;

/// <summary>
/// Encodes which status transitions are legal. Keeping this in one place means the
/// rules can't be bypassed by a stray controller, and they are trivially unit-testable.
/// </summary>
public class TicketStatusWorkflow
{
    private static readonly Dictionary<TicketStatus, HashSet<TicketStatus>> Allowed = new()
    {
        [TicketStatus.Open]       = new() { TicketStatus.InProgress, TicketStatus.Closed },
        [TicketStatus.InProgress] = new() { TicketStatus.Resolved, TicketStatus.Open, TicketStatus.Closed },
        [TicketStatus.Resolved]   = new() { TicketStatus.Closed, TicketStatus.InProgress },
        [TicketStatus.Closed]     = new() { TicketStatus.Open }   // reopen
    };

    public bool CanTransition(TicketStatus from, TicketStatus to)
        => from == to || (Allowed.TryGetValue(from, out var targets) && targets.Contains(to));

    public IReadOnlyCollection<TicketStatus> AllowedNextStatuses(TicketStatus from)
        => Allowed.TryGetValue(from, out var targets) ? targets : new HashSet<TicketStatus>();
}
