using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Models
{
    public class TaskReport
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public DateTime GeneratedDate { get; set; }
        
        [Required]
        public string ReportName { get; set; }
        
        [Required]
        public int TotalTasks { get; set; }
        
        [Required]
        public int CompletedTasks { get; set; }
        
        [Required]
        public int InProgressTasks { get; set; }
        
        [Required]
        public int PendingTasks { get; set; }
        
        [Required]
        public double CompletionRate { get; set; }
        
        public int ProjectId { get; set; }
        
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        
        [Required]
        public string ExportFormat { get; set; }
        
        [Required]
        public string ReportUrl { get; set; }
    }
}