using TicketManagement.Application.Dtos;
using TicketManagement.Domain;

namespace TicketManagement.Infrastructure.Data;

public interface ITicketRepository
{
    Task<Ticket> AddAsync(Ticket ticket);
    Task<Ticket?> GetByIdAsync(Guid id);
    Task<(List<Ticket> items, int total)> QueryAsync(TicketQueryParameters query);
    Task SaveChangesAsync();
}
