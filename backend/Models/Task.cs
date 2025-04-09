using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int ProjectId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public TaskStatus Status { get; set; }
        
        [Required]
        public TaskPriority Priority { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation property
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
    
    public enum TaskStatus
    {
        ToDo = 0,
        InProgress = 1, 
        InReview = 2,
        Done = 3
    }
    
    public enum TaskPriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3
    }
}
