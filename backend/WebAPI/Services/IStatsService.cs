using ProjectManager.Models.DTOs;

namespace ProjectManager.Services
{
    public interface IStatsService
    {
        Task<object> GetDashboardStatsAsync();
    }
}