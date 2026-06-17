using TicketManagement.Application.Dtos;
using TicketManagement.Domain;
using TicketManagement.Infrastructure.Data;

namespace TicketManagement.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repo;
    public ProjectService(IProjectRepository repo) => _repo = repo;

    public async Task<(bool ok, string? error, ProjectResponse? result)> CreateAsync(CreateProjectRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Key))
            return (false, "Name and Key are required.", null);

        var key = request.Key.Trim().ToUpperInvariant();
        if (await _repo.KeyExistsAsync(key))
            return (false, $"A project with key '{key}' already exists.", null);

        var project = new Project { Name = request.Name.Trim(), Key = key };
        await _repo.AddAsync(project);
        return (true, null, ProjectResponse.From(project));
    }

    public async Task<ProjectResponse?> GetAsync(Guid id)
    {
        var p = await _repo.GetByIdAsync(id);
        return p is null ? null : ProjectResponse.From(p);
    }

    public async Task<List<ProjectResponse>> GetAllAsync()
    {
        var projects = await _repo.GetAllAsync();
        return projects.Select(ProjectResponse.From).ToList();
    }
}
