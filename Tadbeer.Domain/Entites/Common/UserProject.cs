using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Projects;
using TaskTracking.Domain.Entites.User;

namespace TaskTracking.Domain.Entites.Common
{
    public class UserProject
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
