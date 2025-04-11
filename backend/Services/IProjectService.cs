using ProjectManager.Models;
using ProjectManager.Models.DTOs;

namespace ProjectManager.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task<Project> CreateProjectAsync(ProjectCreateDto projectDto);
        Task<Project?> UpdateProjectAsync(int id, ProjectUpdateDto projectDto);
        Task<bool> DeleteProjectAsync(int id);
    }
}