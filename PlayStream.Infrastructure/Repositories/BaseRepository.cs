using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayStream.Core.Entities;
using PlayStream.Core.Interfaces;
using PlayStream.Infrastructure.Data;

namespace PlayStream.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly PlayStreamContext _context;
        protected readonly DbSet<T> _entities;

        public BaseRepository(PlayStreamContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }
        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
        }
        public async Task DeleteAsync(int id)
        {
            T entity = await GetByIdAsync(id);
            _entities.Remove(entity);
        }
    }
}