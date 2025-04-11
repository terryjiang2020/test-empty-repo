# Project Management System API Enhancement

## Current System Overview

The current Project Management System consists of:

- .NET Core backend:
  - Models for Project and Task entities
  - RESTful API controllers 
  - SQL database integration with Entity Framework
  - JWT authentication
  
- Vue.js frontend:
  - Dashboard with statistics cards
  - Project and task data tables
  - API service layer for backend communication
  - Vue Router for navigation

## Planned API Enhancement

### API Structure

APIs in this system follow this structure:
- Endpoints in Controllers/**.cs
- Data processing in Services/**.cs
- Data models in Models/**.cs

### New API: Task Reporting Feature

#### Backend Changes

```csharp
// Models/DTOs/TaskReportDto.cs
namespace ProjectManager.Models.DTOs
{
    public class TaskReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? ProjectId { get; set; }
        public string? StatusFilter { get; set; }
        public string? PriorityFilter { get; set; }
    }
}

// Models/TaskReport.cs
namespace ProjectManager.Models
{
    public class TaskReport
    {
        public int Id { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string ReportName { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int PendingTasks { get; set; }
        public double CompletionRate { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public string ExportFormat { get; set; }
        public string ReportUrl { get; set; }
    }
}

// Services/TaskReportService.cs
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
            query = query.Where(t => t.Status == reportRequest.StatusFilter);
        }
        
        if (!string.IsNullOrEmpty(reportRequest.PriorityFilter))
        {
            query = query.Where(t => t.Priority == reportRequest.PriorityFilter);
        }
        
        query = query.Where(t => t.CreatedDate >= reportRequest.StartDate && 
                               t.CreatedDate <= reportRequest.EndDate);
        
        var tasks = await query.ToListAsync();
        
        // Generate report statistics
        var report = new TaskReport
        {
            GeneratedDate = DateTime.UtcNow,
            ReportName = $"Task Report {DateTime.UtcNow:yyyy-MM-dd}",
            TotalTasks = tasks.Count,
            CompletedTasks = tasks.Count(t => t.Status == "Completed"),
            InProgressTasks = tasks.Count(t => t.Status == "In Progress"),
            PendingTasks = tasks.Count(t => t.Status == "Pending"),
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
}

// Controllers/TaskReportController.cs
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TaskReportController : ControllerBase
{
    private readonly ITaskReportService _reportService;
    
    public TaskReportController(ITaskReportService reportService)
    {
        _reportService = reportService;
    }
    
    [HttpPost("generate")]
    public async Task<ActionResult<TaskReport>> GenerateReport(TaskReportDto reportRequest)
    {
        try
        {
            var report = await _reportService.GenerateTaskReport(reportRequest);
            return Ok(report);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskReport>> GetReport(int id)
    {
        var report = await _reportService.GetReportById(id);
        
        if (report == null)
        {
            return NotFound();
        }
        
        return Ok(report);
    }
}
```

#### Frontend Changes

```javascript
// services/api.js - Add these methods
// Task Report API calls
export const generateTaskReport = async (reportParams) => {
  return api.post('/api/TaskReport/generate', reportParams);
};

export const getTaskReport = async (reportId) => {
  return api.get(`/api/TaskReport/${reportId}`);
};

// views/ReportView.vue
// New view for generating and displaying reports
```

### Implementation Notes

When implementing this API enhancement:

1. Ensure all endpoints are properly secured with JWT authentication
2. Add appropriate validation for the TaskReportDto
3. Implement error handling and logging throughout the service
4. Add database migration for the new TaskReport entity
5. Update Swagger documentation for the new endpoints
6. Consider implementing report export functionality (PDF, Excel)
7. Add unit tests for the new service methods

Do not add or install any new libraries. Use the existing libraries in the project.

Do not change anything that is not related to this feature.