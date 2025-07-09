using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Tasks;
using TaskTracking.Domain.Entites.Tasks.Dtos;
using TaskTracking.Domain.Enums;

namespace TaskTracking.Services.Interface.ProjectTasks
{
    public interface IProjectTaskServices
    {
        public Task<List<ProjectTaskDto>> GetAllAsyinc();
        public Task<OperationResult> AddAsync(ProjectTaskDto entity);
        public Task<ProjectTaskDto?> FindByIdAsync(Guid id);
        public Task<OperationResult> DeleteAsync(Guid id);
        public Task<OperationResult> UpdateAsync(Guid id);
        Task<List<ProjectTaskDto>> GetTasksByProjectIdAsync(Guid projectId);
        Task<List<ProjectTaskDto>> GetTasksByUserIdAsync(string userId);
        Task<ProjectTaskDto?> GetTaskWithDetailsAsync(Guid taskId);
        Task<List<ProjectTaskDto>> GetOverdueTasksAsync();
    }
}
