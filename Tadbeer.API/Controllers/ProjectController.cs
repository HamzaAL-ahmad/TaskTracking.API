using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskTracking.Domain.Entites.Projects.Dtos;
using TaskTracking.Domain.Enums;
using TaskTracking.Services.Interface.Projects;

namespace TaskTracking.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;

        public ProjectController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDto projectDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await _projectServices.AddAsync(projectDto, userId);
            if (result == OperationResult.Success)
            {
                return Ok(new { Status = "Success", Message = "Project created successfully!" });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Project creation failed!" });
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var projects = await _projectServices.GetProjectsByUserIdAsync(userId);
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Check if user has access to this project
            var hasAccess = await _projectServices.IsUserInProjectAsync(userId, id);
            if (!hasAccess)
            {
                return Forbid();
            }

            var project = await _projectServices.FindByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return Ok(project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectDto projectDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Check if user has access to this project
            var hasAccess = await _projectServices.IsUserInProjectAsync(userId, id);
            if (!hasAccess)
            {
                return Forbid();
            }

            var result = await _projectServices.UpdateAsync(id);
            if (result == OperationResult.Success)
            {
                return Ok(new { Status = "Success", Message = "Project updated successfully!" });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Project update failed!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Check if user has access to this project
            var hasAccess = await _projectServices.IsUserInProjectAsync(userId, id);
            if (!hasAccess)
            {
                return Forbid();
            }

            var result = await _projectServices.DeleteAsync(id);
            if (result == OperationResult.Success)
            {
                return Ok(new { Status = "Success", Message = "Project deleted successfully!" });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Project deletion failed!" });
        }
    }
}

