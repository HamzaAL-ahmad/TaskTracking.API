using TaskTracking.Domain.Entites.Tasks;
using TaskTracking.Domain.Enums;
using TaskTracking.Domain.Intefaces.Tasks;
using TaskTracking.Domain.Intefaces;
using TaskTracking.Domain.Entites.Tasks.Dtos;
using TaskTracking.Services.Interface.ProjectTasks;

public class ProjectTaskServices : IProjectTaskServices
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectTaskServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> AddAsync(ProjectTaskDto dto)
    {
        try
        {
            var entity = MapToEntity(dto);
            await _unitOfWork.ProjectTaskRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success;
        }
        catch
        {
            return OperationResult.Error;
        }
    }

    public async Task<OperationResult> DeleteAsync(Guid id)
    {
        try
        {
            var task = await _unitOfWork.ProjectTaskRepository.FindByIdAsync(id);
            if (task == null)
            {
                return OperationResult.NotFound;
            }

            await _unitOfWork.ProjectTaskRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success;
        }
        catch
        {
            return OperationResult.Error;
        }
    }

    public async Task<ProjectTaskDto?> FindByIdAsync(Guid id)
    {
        var task = await _unitOfWork.ProjectTaskRepository.GetTaskWithDetailsAsync(id);
        return task != null ? MapToDto(task) : null;
    }

    public async Task<List<ProjectTaskDto>> GetAllAsyinc()
    {
        var tasks = await _unitOfWork.ProjectTaskRepository.GetAllAsyinc();
        return tasks.Select(MapToDto).ToList();
    }

    public async Task<List<ProjectTaskDto>> GetTasksByUserIdAsync(string userId)
    {
        var tasks = await _unitOfWork.ProjectTaskRepository.GetTasksByUserIdAsync(userId);
        return tasks.Select(MapToDto).ToList();
    }

    public async Task<List<ProjectTaskDto>> GetTasksByProjectIdAsync(Guid projectId)
    {
        var tasks = await _unitOfWork.ProjectTaskRepository.GetTasksByProjectIdAsync(projectId);
        return tasks.Select(MapToDto).ToList();
    }

    public async Task<List<ProjectTaskDto>> GetOverdueTasksAsync()
    {
        var tasks = await _unitOfWork.ProjectTaskRepository.GetOverdueTasksAsync();
        return tasks.Select(MapToDto).ToList();
    }

    public async Task<ProjectTaskDto?> GetTaskWithDetailsAsync(Guid taskId)
    {
        var task = await _unitOfWork.ProjectTaskRepository.GetTaskWithDetailsAsync(taskId);
        return task != null ? MapToDto(task) : null;
    }

    public async Task<OperationResult> UpdateAsync(Guid id)
    {
        try
        {
            await _unitOfWork.ProjectTaskRepository.UpdateAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success;
        }
        catch
        {
            return OperationResult.Error;
        }
    }

    // ========== Manual Mapping ==========

    private ProjectTaskDto MapToDto(ProjectTask entity)
    {
        return new ProjectTaskDto
        {
            Id = entity.Id,
            Title = entity.Title,
            description = entity.description,
            DueDate = entity.DueDate,
            Completion = entity.Completion,
            status = entity.status,
            UserId = entity.UserId,
            UserInfo = entity.UserInfo,
            ProjectId = entity.ProjectId,
            Project = entity.Project
        };
    }

    private ProjectTask MapToEntity(ProjectTaskDto dto)
    {
        return new ProjectTask
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Title = dto.Title,
            description = dto.description,
            DueDate = dto.DueDate,
            Completion = dto.Completion,
            status = dto.status,
            UserId = dto.UserId,
            ProjectId = dto.ProjectId
        };
    }
}
