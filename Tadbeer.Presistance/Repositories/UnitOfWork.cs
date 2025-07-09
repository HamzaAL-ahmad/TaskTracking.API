using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Intefaces;
using TaskTracking.Domain.Intefaces.Projects;
using TaskTracking.Domain.Intefaces.Tasks;
using TaskTracking.Presistance.Repositories.Projects;
using TaskTracking.Presistance.Repositories.Tasks;
using TaskTracking.Presistance.SQL;

namespace TaskTracking.Presistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskTrackingContext _context;

        public IProjectTaskRepostroy ProjectTaskRepository { get; private set; }
        public IProjectRepository ProjectRepository { get; private set; }

  

        public UnitOfWork(TaskTrackingContext context)
        {
            _context = context;
            ProjectTaskRepository = new ProjectTaskRepository(context);
            ProjectRepository = new ProjectRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        Task IUnitOfWork.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}
