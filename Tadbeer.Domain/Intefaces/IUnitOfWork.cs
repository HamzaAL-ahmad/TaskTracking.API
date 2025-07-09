using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites.Projects;
using TaskTracking.Domain.Entites.Tasks;
using TaskTracking.Domain.Entites.User;
using TaskTracking.Domain.Intefaces.Projects;
using TaskTracking.Domain.Intefaces.Tasks;

namespace TaskTracking.Domain.Intefaces
{
    public interface IUnitOfWork
    {
        public IProjectTaskRepostroy ProjectTaskRepository { get;  }
        public IProjectRepository ProjectRepository { get; }

        public void SaveChanges();
        Task SaveChangesAsync();
    }
}
