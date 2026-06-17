using TicketManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace TicketManagement.Infrastructure.Data;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _db;
    public ProjectRepository(AppDbContext db) => _db = db;

    public async Task<Project> AddAsync(Project project)
    {
        await _db.Projects.AddAsync(project);
        await _db.SaveChangesAsync();
        return project;
    }

    public async Task<Project?> GetByIdAsync(Guid id) =>
        await _db.Projects.Include(p => p.Tickets)
                          .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<List<Project>> GetAllAsync() =>
        await _db.Projects.Include(p => p.Tickets)
                          .OrderByDescending(p => p.CreatedAt)
                          .ToListAsync();

    public async Task<bool> KeyExistsAsync(string key) =>
        await _db.Projects.AnyAsync(p => p.Key == key);
}
