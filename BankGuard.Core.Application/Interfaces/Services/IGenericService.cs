namespace BankGuard.Core.Application.Interfaces.Services
{
    public interface IGenericService<SaveViewModel, ViewModel, Entity, type>
        where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        Task<ViewModel> Add(SaveViewModel save);
        Task Delete(type id);
        Task<List<ViewModel>> GetAll();
        Task<SaveViewModel> GetById(type id);
        Task Update(SaveViewModel model, type id);
    }
}