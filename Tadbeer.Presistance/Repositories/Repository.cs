using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracking.Domain.Entites;
using TaskTracking.Domain.Enums;
using TaskTracking.Domain.Intefaces;
using TaskTracking.Presistance.SQL;

namespace TaskTracking.Presistance.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly TaskTrackingContext _context;
        private readonly DbSet<TEntity> _entities;

        public Repository(TaskTrackingContext context)
        {
            _context = context;
            _entities = _context.Set<TEntity>();
        }

        public async Task<OperationResult> AddAsync(TEntity entity)
        {
            try
            {
                await _entities.AddAsync(entity);
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Error;
            }
        }

        public async Task<OperationResult> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _entities.FindAsync(id);
                if (entity == null)
                    return OperationResult.Error;

                _entities.Remove(entity);
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Error;
            }
        }

        public async Task<TEntity?> FindByIdAsync(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<List<TEntity>> GetAllAsyinc()
        {
            return await _entities.ToListAsync();
        }

       
        public async Task<OperationResult> UpdateAsync(Guid id)
        {
            try
            {
                var entity = await _entities.FindAsync(id);
                if (entity == null)
                    return OperationResult.Error;

                _context.Entry(entity).State = EntityState.Modified;
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Error;
            }
        }
    }

}
