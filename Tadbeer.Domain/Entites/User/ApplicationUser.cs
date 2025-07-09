using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Common;
using TaskTracking.Domain.Entites.Projects;
using TaskTracking.Domain.Entites.Tasks;

namespace TaskTracking.Domain.Entites.User
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public string Department { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public bool IsActive { get; set; } = true;
        public ICollection<ProjectTask> Tasks { get; set; }
        public ICollection<UserProject> UserProjects { get; set;}
    }
}
