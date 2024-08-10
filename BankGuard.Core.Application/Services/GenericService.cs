using AutoMapper;
using BankGuard.Core.Application.Interfaces.Repositories;
using BankGuard.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuard.Core.Application.Services
{
    public class GenericService<SaveViewModel, ViewModel, Entity, type> : IGenericService<SaveViewModel, ViewModel, Entity, type> where SaveViewModel : class
        where ViewModel : class
        where Entity : class
    {
        private readonly IGenericRepository<Entity,type> _repository;
        private readonly IMapper _mapper;
        public GenericService(IGenericRepository<Entity, type> generic, IMapper mapper)
        {
            _repository = generic;
            _mapper = mapper;
        }

        public virtual async Task<List<ViewModel>> GetAll()
        {
            var entity = await _repository.GetAllAsync();
            return _mapper.Map<List<ViewModel>>(entity).ToList();
        }
        public virtual async Task<SaveViewModel> GetById(type id)
        {
            Entity entity = await _repository.GetById(id);
            return _mapper.Map<SaveViewModel>(entity);
        }
        public virtual async Task<ViewModel> Add(SaveViewModel save)
        {
            var entry = _mapper.Map<Entity>(save);
            Entity entity = await _repository.AddAsync(entry);
            return _mapper.Map<ViewModel>(entry);
        }
        public virtual async Task Update(SaveViewModel model, type id)
        {
            var entry = _mapper.Map<Entity>(model);
            await _repository.UpdateAsync(entry, id);
        }
        public virtual async Task Delete(type id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
