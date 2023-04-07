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
    public class SubMenuService : ISubMenuService
    {
        private readonly ISubMenuRepository _subMenuRepository;
        public SubMenuService(ISubMenuRepository subMenuRepository)
        {
            _subMenuRepository = subMenuRepository;
        }

        public async Task<int> Delete(MstSubmenu model)
        {
            return await _subMenuRepository.Delete(model);
        }

        public async Task<IEnumerable<MstSubmenu>> GetAll()
        {
            return await _subMenuRepository.GetAll();
        }

        public async Task<IEnumerable<MstSubmenu>> GetById(Guid id)
        {
            return await _subMenuRepository.GetById(id);
        }

        public async Task<int> Insert(MstSubmenu model)
        {
            return await _subMenuRepository.Insert(model);
        }

        public Task<int> Update(MstSubmenu model)
        {
            return _subMenuRepository.Update(model);
        }
    }
}
