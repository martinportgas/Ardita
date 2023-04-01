using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public Task<int> Delete(MstMenu model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MstMenu>> GetAll()
        {
            return _menuRepository.GetAll();
        }

        public Task<IEnumerable<MstMenu>> GetById(Guid id)
        {
            return _menuRepository.GetById(id);
        }

        public Task<int> Insert(MstMenu model)
        {
            return _menuRepository.Insert(model);
        }

        public Task<int> Update(MstMenu model)
        {
            return _menuRepository.Update(model);
        }
    }
}
