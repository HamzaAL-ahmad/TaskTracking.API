using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Projects;
using TaskTracking.Domain.Entites.Projects.Dtos;
using TaskTracking.Domain.Enums;

namespace TaskTracking.Services.Interface.Projects
{
    public interface IProjectServices
    {
        public Task<List<ProjectDto>> GetAllAsyinc();
        public Task<OperationResult> AddAsync(ProjectDto entity, string userId);
        public Task<ProjectDto> FindByIdAsync(Guid id);
        public Task<OperationResult> DeleteAsync(Guid id);
        public Task<OperationResult> UpdateAsync(Guid id);
        Task<List<ProjectDto>> GetProjectsByUserIdAsync(string userId);
        Task<ProjectDto> GetProjectWithTasksAsync(Guid projectId);
        Task<bool> IsUserInProjectAsync(string userId, Guid projectId);
    }
}
