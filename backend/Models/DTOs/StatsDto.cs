namespace ProjectManager.Models.DTOs
{
    public class StatsDto
    {
        public string Label { get; set; } = string.Empty;
        public int Value { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}