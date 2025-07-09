using TaskTracking.Domain.Entites.Projects;
using TaskTracking.Domain.Entites.Projects.Dtos;
using TaskTracking.Domain.Entites.Tasks.Dtos;
using TaskTracking.Domain.Enums;
using TaskTracking.Domain.Intefaces;
using TaskTracking.Services.Interface.Projects;

namespace TaskTracking.Services.Services.Projects
{
    public class ProjectServices : IProjectServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> AddAsync(ProjectDto entity, string userId)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Title = entity.Title,
                description = entity.description,
                DueDate = entity.DueDate,
                Completion = entity.Completion,
                status = entity.status,
            };
            
            await _unitOfWork.ProjectRepository.AddAsync(project);
            
            // Create UserProject relationship
            await _unitOfWork.ProjectRepository.CreateUserProjectAsync(userId, project.Id);
            await _unitOfWork.SaveChangesAsync();

            return OperationResult.Success;
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            await _unitOfWork.ProjectRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success;
        }

        public async Task<ProjectDto> FindByIdAsync(Guid id)
        {
            var project = await _unitOfWork.ProjectRepository.FindByIdAsync(id);
            return new ProjectDto
            {
                description = project.description,
                Title = project.Title,
                DueDate = project.DueDate,
                Completion = project.Completion,
                status = project.status
            };
        }

        public async Task<List<ProjectDto>> GetAllAsyinc()
        {
            var dtos = new List<ProjectDto>();
            var projects = await _unitOfWork.ProjectRepository.GetAllAsyinc();
            foreach (var item in projects)
            {
                dtos.Add(new ProjectDto
                {
                    description = item.description,
                    Title = item.Title,
                    DueDate = item.DueDate,
                    Completion = item.Completion,
                    status = item.status
                });
            }
            return dtos;
        }

        public async Task<List<ProjectDto>> GetProjectsByUserIdAsync(string userId)
        {
            var dtos = new List<ProjectDto>();
            var projects = await _unitOfWork.ProjectRepository.GetProjectsByUserIdAsync(userId);
            foreach (var item in projects)
            {
                dtos.Add(new ProjectDto
                {
                    description = item.description,
                    Title = item.Title,
                    DueDate = item.DueDate,
                    Completion = item.Completion,
                    status = item.status,
                    projectTaskDtos = item.Tasks?.Select(x => new ProjectTaskDto
                    {
                        description = x.description,
                        DueDate = x.DueDate,
                        status = x.status,
                        Title = x.Title,
                        Completion = x.Completion,
                    }).ToList(),

                });
            }
            return dtos;
        }

        public async Task<ProjectDto> GetProjectWithTasksAsync(Guid projectId)
        {
            var project = await _unitOfWork.ProjectRepository.GetProjectWithTasksAsync(projectId);
            var dto = new ProjectDto
            {
                description = project.description,
                Title = project.Title,
                DueDate = project.DueDate,
                Completion = project.Completion,
                status = project.status,
                projectTaskDtos = project.Tasks?.Select(x => new ProjectTaskDto
                {
                    description = x.description,
                    DueDate = x.DueDate,
                    status = x.status,
                    Title = x.Title,
                    Completion = x.Completion,
                }).ToList(),
            };

            return dto;
        }

        public async Task<bool> IsUserInProjectAsync(string userId, Guid projectId)
        {
            var IsUserInProjectAsync = await _unitOfWork.ProjectRepository.IsUserInProjectAsync(userId, projectId);
            return IsUserInProjectAsync;
        }

        public async Task<OperationResult> UpdateAsync(Guid id)
        {
            await _unitOfWork.ProjectRepository.UpdateAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success;
        }
    }
}
