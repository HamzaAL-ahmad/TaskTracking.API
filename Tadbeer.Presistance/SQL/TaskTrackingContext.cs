using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Common;
using TaskTracking.Domain.Entites.Projects;
using TaskTracking.Domain.Entites.Tasks;

namespace TaskTracking.Presistance.SQL
{
    public class TaskTrackingContext:IdentityDbContext
    {
        public TaskTrackingContext(DbContextOptions<TaskTrackingContext> dbContextOptions):base(dbContextOptions)
        {
        }
        public DbSet<Project> projects { get; set; }
        public DbSet<ProjectTask> tasks { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserProject>()
                .HasKey(up => new { up.UserId, up.ProjectId });

            builder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(up => up.UserId);

            builder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(up => up.ProjectId);
        }
    }
}
