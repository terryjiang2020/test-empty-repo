using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;
using ProjectManager.Models.Auth;

namespace ProjectManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Soft delete for Projects
            modelBuilder.Entity<Project>().HasQueryFilter(p => !p.IsDeleted);
            
            // Soft delete for Tasks
            modelBuilder.Entity<Task>().HasQueryFilter(t => !t.IsDeleted);
            
            // Project-Task relationship
            modelBuilder.Entity<Task>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}