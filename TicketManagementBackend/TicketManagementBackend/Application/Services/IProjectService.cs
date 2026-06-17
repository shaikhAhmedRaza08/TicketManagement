using TicketManagement.Application.Dtos;

namespace TicketManagement.Application.Services;

public interface IProjectService
{
    Task<(bool ok, string? error, ProjectResponse? result)> CreateAsync(CreateProjectRequest request);
    Task<ProjectResponse?> GetAsync(Guid id);
    Task<List<ProjectResponse>> GetAllAsync();
}
