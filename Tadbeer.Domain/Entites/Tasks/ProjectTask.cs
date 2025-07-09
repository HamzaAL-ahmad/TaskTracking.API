using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Projects;
using TaskTracking.Domain.Entites.User;

namespace TaskTracking.Domain.Entites.Tasks
{
    public class ProjectTask: BaseEntity
    {
        public string AssignedTo { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser UserInfo { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
