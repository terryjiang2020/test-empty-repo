using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;
using ProjectManager.Models.DTOs;

namespace ProjectManager.Services
{
    public class TaskReportService : ITaskReportService
    {
        private readonly ApplicationDbContext _context;
        
        public TaskReportService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<TaskReport> GenerateTaskReport(TaskReportDto reportRequest)
        {
            // Query tasks based on filter criteria
            var query = _context.Tasks.AsQueryable();
            
            if (reportRequest.ProjectId.HasValue)
            {
                query = query.Where(t => t.ProjectId == reportRequest.ProjectId.Value);
            }
            
            if (!string.IsNullOrEmpty(reportRequest.StatusFilter))
            {
                query = query.Where(t => t.Status.ToString() == reportRequest.StatusFilter);
            }
            
            if (!string.IsNullOrEmpty(reportRequest.PriorityFilter))
            {
                query = query.Where(t => t.Priority.ToString() == reportRequest.PriorityFilter);
            }
            
            query = query.Where(t => t.CreatedAt >= reportRequest.StartDate && 
                                   t.CreatedAt <= reportRequest.EndDate);
            
            var tasks = await query.ToListAsync();
            
            // Generate report statistics
            var report = new TaskReport
            {
                GeneratedDate = DateTime.UtcNow,
                ReportName = $"Task Report {DateTime.UtcNow:yyyy-MM-dd}",
                TotalTasks = tasks.Count,
                CompletedTasks = tasks.Count(t => t.Status == TaskStatus.Done),
                InProgressTasks = tasks.Count(t => t.Status == TaskStatus.InProgress),
                PendingTasks = tasks.Count(t => t.Status == TaskStatus.ToDo),
                ProjectId = reportRequest.ProjectId ?? 0,
                ExportFormat = "PDF",
                ReportUrl = $"/reports/tasks/{Guid.NewGuid()}.pdf"
            };
            
            report.CompletionRate = report.TotalTasks > 0 
                ? (double)report.CompletedTasks / report.TotalTasks * 100 
                : 0;
            
            // Save report to database
            _context.TaskReports.Add(report);
            await _context.SaveChangesAsync();
            
            return report;
        }

        public async Task<TaskReport> GetReportById(int id)
        {
            return await _context.TaskReports
                .Include(r => r.Project)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}