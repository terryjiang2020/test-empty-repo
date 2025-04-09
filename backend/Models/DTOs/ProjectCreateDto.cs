using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models.DTOs
{
    public class ProjectCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
    }
}
