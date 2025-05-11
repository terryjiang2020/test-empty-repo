using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;

namespace ProjectManager.Services
{
    public class StatsService : IStatsService
    {
        private readonly ApplicationDbContext _context;

        public StatsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<object> GetDashboardStatsAsync()
        {
            // Total projects count
            var totalProjects = await _context.Projects.CountAsync();
            
            // Projects by status
            var projectsByStatus = await _context.Projects
                .GroupBy(p => p.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();
            
            // Total tasks count
            var totalTasks = await _context.Tasks.CountAsync();
            
            // Tasks by status
            var tasksByStatus = await _context.Tasks
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();
            
            // Tasks by priority
            var tasksByPriority = await _context.Tasks
                .GroupBy(t => t.Priority)
                .Select(g => new { Priority = g.Key, Count = g.Count() })
                .ToListAsync();
            
            // Recent projects
            var recentProjects = await _context.Projects
                .OrderByDescending(p => p.CreatedAt)
                .Take(5)
                .Select(p => new 
                { 
                    p.Id, 
                    p.Name, 
                    p.Status, 
                    p.CreatedAt,
                    TasksCount = _context.Tasks.Count(t => t.ProjectId == p.Id)
                })
                .ToListAsync();

            return new
            {
                TotalProjects = totalProjects,
                ProjectsByStatus = projectsByStatus,
                TotalTasks = totalTasks,
                TasksByStatus = tasksByStatus,
                TasksByPriority = tasksByPriority,
                RecentProjects = recentProjects
            };
        }
    }
}