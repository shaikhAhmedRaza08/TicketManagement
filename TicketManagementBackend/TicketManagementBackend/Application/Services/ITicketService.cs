using TicketManagement.Application.Dtos;

namespace TicketManagement.Application.Services;

public interface ITicketService
{
    Task<(bool ok, string? error, TicketResponse? result)> CreateAsync(CreateTicketRequest request);
    Task<TicketResponse?> GetAsync(Guid id);
    Task<PagedResult<TicketListItemResponse>> QueryAsync(TicketQueryParameters query);
    Task<(bool ok, string? error, TicketResponse? result)> UpdateAsync(Guid id, UpdateTicketRequest request);
    Task<(bool ok, string? error, TicketResponse? result)> ChangeStatusAsync(Guid id, ChangeStatusRequest request);
    Task<(bool ok, string? error, CommentResponse? result)> AddCommentAsync(Guid ticketId, CreateCommentRequest request);
}
