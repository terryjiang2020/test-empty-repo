using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;
using ProjectManager.Models.DTOs;

namespace ProjectManager.Services
{
    public class StatsService : IStatsService
    {
        private readonly ApplicationDbContext _context;

        public StatsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var activeProjects = await _context.Projects
                .Where(p => !p.IsDeleted && p.Status == ProjectStatus.Active)
                .CountAsync();

            var completedProjects = await _context.Projects
                .Where(p => !p.IsDeleted && p.Status == ProjectStatus.Completed)
                .CountAsync();

            var totalProjects = await _context.Projects
                .Where(p => !p.IsDeleted)
                .CountAsync();

            var totalTasks = await _context.Tasks
                .Where(t => !t.IsDeleted)
                .CountAsync();

            var completedTasks = await _context.Tasks
                .Where(t => !t.IsDeleted && t.Status == TaskStatus.Done)
                .CountAsync();

            // Get project counts by status
            var projectsByStatus = await _context.Projects
                .Where(p => !p.IsDeleted)
                .GroupBy(p => p.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status.ToString(), x => x.Count);

            // Get task counts by status
            var tasksByStatus = await _context.Tasks
                .Where(t => !t.IsDeleted)
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Status.ToString(), x => x.Count);

            return new DashboardStatsDto
            {
                TotalProjects = totalProjects,
                ActiveProjects = activeProjects,
                CompletedProjects = completedProjects,
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                ProjectsByStatus = projectsByStatus,
                TasksByStatus = tasksByStatus
            };
        }
    }
}