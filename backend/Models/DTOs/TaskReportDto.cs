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