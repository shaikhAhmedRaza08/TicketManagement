using TicketManagement.Application.Dtos;
using TicketManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace TicketManagement.Infrastructure.Data;

public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _db;
    public TicketRepository(AppDbContext db) => _db = db;

    public async Task<Ticket> AddAsync(Ticket ticket)
    {
        await _db.Tickets.AddAsync(ticket);
        await _db.SaveChangesAsync();
        return ticket;
    }

    public async Task<Ticket?> GetByIdAsync(Guid id) =>
        await _db.Tickets.Include(t => t.Comments)
                         .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<(List<Ticket> items, int total)> QueryAsync(TicketQueryParameters q)
    {
        // Build the query incrementally - each filter is applied only when supplied.
        IQueryable<Ticket> query = _db.Tickets.AsNoTracking();

        if (q.ProjectId is { } projectId)
            query = query.Where(t => t.ProjectId == projectId);

        if (q.Status is { } status)
            query = query.Where(t => t.Status == status);

        if (q.Priority is { } priority)
            query = query.Where(t => t.Priority == priority);

        if (!string.IsNullOrWhiteSpace(q.Assignee))
            query = query.Where(t => t.Assignee == q.Assignee);

        if (!string.IsNullOrWhiteSpace(q.Search))
        {
            var term = q.Search.Trim();
            query = query.Where(t => t.Title.Contains(term) || t.Description.Contains(term));
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
