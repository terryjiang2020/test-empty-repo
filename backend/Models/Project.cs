using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public ProjectStatus Status { get; set; }
        
        [Required]
        public bool IsDeleted { get; set; } = false;
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation property
        public ICollection<Task>? Tasks { get; set; }
    }
    
    public enum ProjectStatus
    {
        Active = 0,
        OnHold = 1,
        Completed = 2,
        Cancelled = 3
    }
}
