using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Projects;
using TaskTracking.Domain.Entites.User;

namespace TaskTracking.Domain.Entites.Tasks.Dtos
{
    public class ProjectTaskDto
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; }
        public string? description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public DateTimeOffset Completion { get; set; }
        public int status { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser UserInfo { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
