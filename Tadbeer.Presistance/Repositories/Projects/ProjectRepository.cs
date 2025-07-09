using Microsoft.EntityFrameworkCore;
using TaskTracking.Domain.Entites.Projects;
using TaskTracking.Domain.Enums;
using TaskTracking.Domain.Intefaces;
using TaskTracking.Domain.Intefaces.Projects;
using TaskTracking.Presistance.SQL;

namespace TaskTracking.Presistance.Repositories.Projects
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private readonly TaskTrackingContext _context;


        public ProjectRepository(TaskTrackingContext context) : base(context)
        {
            _context = context;

        }


        public async Task<List<Project>> GetProjectsByUserIdAsync(string userId)
        {
            return await _context.projects
                .Include(p => p.UserProjects)
                .Include(p => p.Tasks)
                .Where(p => p.UserProjects.Any(up => up.UserId == userId))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Project> GetProjectWithTasksAsync(Guid projectId)
        {
            return await _context.projects.Where(p => p.Id == projectId)
                          .Include(p => p.Tasks)
                          .AsNoTracking()
                          .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserInProjectAsync(string userId, Guid projectId)
        {
            return await _context.UserProjects
                       .AnyAsync(up => up.UserId == userId && up.ProjectId == projectId);
        }

        public async Task<OperationResult> CreateUserProjectAsync(string userId, Guid projectId)
        {
            try
            {
                var userProject = new TaskTracking.Domain.Entites.Common.UserProject
                {
                    UserId = userId,
                    ProjectId = projectId
                };

                await _context.UserProjects.AddAsync(userProject);
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Error;
            }
        }



    }
}

