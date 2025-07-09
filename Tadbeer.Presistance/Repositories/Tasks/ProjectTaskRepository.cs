using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Tasks;
using TaskTracking.Domain.Enums;
using TaskTracking.Domain.Intefaces;
using TaskTracking.Domain.Intefaces.Tasks;
using TaskTracking.Presistance.SQL;

namespace TaskTracking.Presistance.Repositories.Tasks
{

    public class ProjectTaskRepository :Repository<ProjectTask>, IProjectTaskRepostroy
    {
        private readonly TaskTrackingContext _context;
        private readonly DbSet<ProjectTask> _entities;

        public ProjectTaskRepository(TaskTrackingContext context):base(context)
        {
            _context = context;
            _entities = _context.Set<ProjectTask>();
        }


        public async Task<List<ProjectTask>> GetAllAsyinc()
        {
            return await _entities
                .Include(t => t.Project)
                .Include(t => t.UserInfo)
                .ToListAsync();
        }

        public async Task<List<ProjectTask>> GetOverdueTasksAsync()
        {
            return await _entities
                .Where(t => t.DueDate < DateTime.UtcNow && t.status != (int)ActivityStatus.Completed)
                .ToListAsync();
        }

        public async Task<List<ProjectTask>> GetTasksByProjectIdAsync(Guid projectId)
        {
            return await _entities
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.UserInfo)
                .ToListAsync();
        }

        public async Task<List<ProjectTask>> GetTasksByUserIdAsync(string userId)
        {
            return await _entities
                .Where(t => t.UserId == Guid.Parse(userId))
                .Include(t => t.Project)
                .ToListAsync();
        }

        public async Task<ProjectTask> GetTaskWithDetailsAsync(Guid taskId)
        {
            return await _entities
                .Include(t => t.Project)
                .Include(t => t.UserInfo)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }

    }

}

