using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models.DTOs
{
    public class ProjectUpdateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public ProjectStatus Status { get; set; }
    }
}
