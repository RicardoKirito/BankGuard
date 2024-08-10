namespace BankGuard.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<Entity, type> where Entity : class
    {
        Task<Entity> AddAsync(Entity entity);
        Task DeleteAsync(type id);
        Task<List<Entity>> GetAllAsync();
        Task<Entity> GetById(type id);
        Task UpdateAsync(Entity entry, type id);
    }
}