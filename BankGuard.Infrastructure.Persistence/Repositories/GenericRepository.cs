using BankGuard.Core.Application.Interfaces.Repositories;
using BankGuard.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<Entity, type> : IGenericRepository<Entity, type> where Entity : class
    {
        public readonly ApplicationContext _context;
        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }
        public virtual async Task<List<Entity>> GetAllAsync()
        {
            return _context.Set<Entity>().ToList();
        }
        public virtual async Task<Entity> GetById(type id)
        {
            return await _context.Set<Entity>().FindAsync(id);
        }
        public virtual async Task<Entity> AddAsync(Entity entity)
        {
             EntityEntry<Entity> entry = await _context.Set<Entity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entry.Entity;

        }
        public virtual async Task UpdateAsync(Entity entry, type id)
        {
            Entity entity = await _context.Set<Entity>().FindAsync(id);
            _context.Entry<Entity>(entity).CurrentValues.SetValues(entry);
            await _context.SaveChangesAsync();
        }
        public virtual  async Task DeleteAsync(type id)
        {
            Entity entry = await _context.Set<Entity>().FindAsync(id);
            _context.Set<Entity>().Remove(entry);
            await _context.SaveChangesAsync();

        }



    }
}
