using TicketManagement.Domain;

namespace TicketManagement.Infrastructure.Data;

public interface IProjectRepository
{
    Task<Project> AddAsync(Project project);
    Task<Project?> GetByIdAsync(Guid id);
    Task<List<Project>> GetAllAsync();
    Task<bool> KeyExistsAsync(string key);
}
