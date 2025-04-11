using ProjectManager.Models;
using ProjectManager.Models.DTOs;

namespace ProjectManager.Services
{
    public interface ITaskReportService
    {
        Task<TaskReport> GenerateTaskReport(TaskReportDto reportRequest);
        Task<TaskReport> GetReportById(int id);
    }
}