using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Projects;
using TaskTracking.Domain.Enums;

namespace TaskTracking.Domain.Intefaces.Projects
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<List<Project>> GetProjectsByUserIdAsync(string userId);
        Task<Project> GetProjectWithTasksAsync(Guid projectId);
        Task<bool> IsUserInProjectAsync(string userId, Guid projectId);
        Task<OperationResult> CreateUserProjectAsync(string userId, Guid projectId);
    }
}
