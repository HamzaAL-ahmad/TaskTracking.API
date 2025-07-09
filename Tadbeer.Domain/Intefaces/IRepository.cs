using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites;
using TaskTracking.Domain.Enums;

namespace TaskTracking.Domain.Intefaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        public Task<List<TEntity>> GetAllAsyinc();
        public Task<OperationResult> AddAsync(TEntity entity);
        public Task<TEntity> FindByIdAsync(Guid id);
        public Task<OperationResult> DeleteAsync(Guid id);
        public Task<OperationResult> UpdateAsync(Guid id);
    }
}
