using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Common;
using TaskTracking.Domain.Entites.Tasks;
using TaskTracking.Domain.Entites.User;

namespace TaskTracking.Domain.Entites.Projects
{
    public class Project: BaseEntity
    {
        public ICollection<UserProject> UserProjects { get; set; }
        public ICollection<ProjectTask> Tasks { get; set; }
    }
}
