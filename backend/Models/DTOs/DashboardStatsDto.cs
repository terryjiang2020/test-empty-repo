namespace ProjectManager.Models.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalProjects { get; set; }
        public int ActiveProjects { get; set; }
        public int CompletedProjects { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public Dictionary<string, int> ProjectsByStatus { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> TasksByStatus { get; set; } = new Dictionary<string, int>();
    }
}