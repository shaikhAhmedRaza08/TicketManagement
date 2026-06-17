using TicketManagement.Application.Workflow;
using TicketManagement.Domain;
using Xunit;

namespace TicketManagement.Tests;

public class TicketStatusWorkflowTests
{
    private readonly TicketStatusWorkflow _workflow = new();

    [Theory]
    [InlineData(TicketStatus.Open, TicketStatus.InProgress)]
    [InlineData(TicketStatus.InProgress, TicketStatus.Resolved)]
    [InlineData(TicketStatus.Resolved, TicketStatus.Closed)]
    [InlineData(TicketStatus.Closed, TicketStatus.Open)]   // reopen
    public void Allows_valid_transitions(TicketStatus from, TicketStatus to)
        => Assert.True(_workflow.CanTransition(from, to));

    [Theory]
    [InlineData(TicketStatus.Open, TicketStatus.Resolved)]   // must go through InProgress
    [InlineData(TicketStatus.Closed, TicketStatus.Resolved)]
    [InlineData(TicketStatus.Resolved, TicketStatus.Open)]
    public void Rejects_invalid_transitions(TicketStatus from, TicketStatus to)
        => Assert.False(_workflow.CanTransition(from, to));

    [Fact]
    public void Same_status_is_always_allowed()
        => Assert.True(_workflow.CanTransition(TicketStatus.Open, TicketStatus.Open));
}
