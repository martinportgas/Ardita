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
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<int> Delete(MstRole model)
        {
            return await _roleRepository.Delete(model);
        }

        public async Task<IEnumerable<MstRole>> GetAll()
        {
            return await _roleRepository.GetAll();
        }

        public async Task<IEnumerable<MstRole>> GetById(Guid id)
        {
            return await _roleRepository.GetById(id);
        }

        public async Task<int> Insert(MstRole model)
        {
            return await _roleRepository.Insert(model);
        }

        public async Task<int> Update(MstRole model)
        {
            return await _roleRepository.Update(model);
        }
    }
}
