using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites;
using TaskTracking.Domain.Entites.Tasks;

namespace TaskTracking.Domain.Intefaces.Tasks
{
    public interface IProjectTaskRepostroy:IRepository<ProjectTask>
    {
        Task<List<ProjectTask>> GetTasksByProjectIdAsync(Guid projectId);
        Task<List<ProjectTask>> GetTasksByUserIdAsync(string userId);
        Task<ProjectTask> GetTaskWithDetailsAsync(Guid taskId);
        Task<List<ProjectTask>> GetOverdueTasksAsync();

    }
}
