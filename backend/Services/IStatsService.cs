using ProjectManager.Models.DTOs;

namespace ProjectManager.Services
{
    public interface IStatsService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}