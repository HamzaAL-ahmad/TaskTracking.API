using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Projects.Dtos;
using TaskTracking.Domain.Entites.Tasks.Dtos;

namespace TaskTracking.Domain.Entites.User.Dtos
{
    public class ApplicationUserDto
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? Department { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public bool IsActive { get; set; } = true;
        List<ProjectDto>? ProjectDtos { get; set; }
        List<ProjectTaskDto>? projectTaskDtos { get; set; }
    }
}
