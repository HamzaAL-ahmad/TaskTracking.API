using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskTracking.Domain.Entites.Tasks.Dtos;
using TaskTracking.Domain.Enums;
using TaskTracking.Services.Interface.ProjectTasks;

namespace TaskTracking.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IProjectTaskServices _taskServices;

        public TaskController(IProjectTaskServices taskServices)
        {
            _taskServices = taskServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] ProjectTaskDto taskDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Set the user ID for the task
            taskDto.UserId = Guid.Parse(userId);

            var result = await _taskServices.AddAsync(taskDto);
            if (result == OperationResult.Success)
            {
                return Ok(new { Status = "Success", Message = "Task created successfully!" });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Task creation failed!" });
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProject(Guid projectId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var tasks = await _taskServices.GetTasksByProjectIdAsync(projectId);
            return Ok(tasks);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetTasksByUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var tasks = await _taskServices.GetTasksByUserIdAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var task = await _taskServices.FindByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            // Check if the task belongs to the current user
            if (task.UserId.ToString() != userId)
            {
                return Forbid();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] ProjectTaskDto taskDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Check if the task belongs to the current user
            var existingTask = await _taskServices.FindByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            if (existingTask.UserId.ToString() != userId)
            {
                return Forbid();
            }

            var result = await _taskServices.UpdateAsync(id);
            if (result == OperationResult.Success)
            {
                return Ok(new { Status = "Success", Message = "Task updated successfully!" });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Task update failed!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Check if the task belongs to the current user
            var existingTask = await _taskServices.FindByIdAsync(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            if (existingTask.UserId.ToString() != userId)
            {
                return Forbid();
            }

            var result = await _taskServices.DeleteAsync(id);
            if (result == OperationResult.Success)
            {
                return Ok(new { Status = "Success", Message = "Task deleted successfully!" });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Task deletion failed!" });
        }

        [HttpGet("overdue")]
        public async Task<IActionResult> GetOverdueTasks()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var tasks = await _taskServices.GetOverdueTasksAsync();
            // Filter tasks for the current user
            var userTasks = tasks.Where(t => t.UserId.ToString() == userId).ToList();
            return Ok(userTasks);
        }
    }
}

