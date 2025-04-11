using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Models;
using ProjectManager.Models.DTOs;
using ProjectManager.Services;

namespace ProjectManager.Controllers
{
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
}