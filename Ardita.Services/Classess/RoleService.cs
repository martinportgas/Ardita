using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Positions;
using Ardita.Models.ViewModels.Roles;
using Ardita.Repositories.Classess;
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

        public async Task<RoleListViewModel> GetListRole(DataTableModel tableModel)
        {
            var roleListViewModel = new RoleListViewModel();

            var roleResult = await _roleRepository.GetAll();
            var results = (from role in roleResult
                           select new RoleListViewDetailModel
                           {
                               RoleId = role.RoleId,
                               RoleCode = role.Code,
                               RoleName = role.Name,
                               IsActive = role.IsActive
                           });

            if (!string.IsNullOrEmpty(tableModel.searchValue))
            {
                results = results.Where(
                    x =>
                    (x.RoleCode != null ? x.RoleCode.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.RoleName != null ? x.RoleName.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                );
            }

            tableModel.recordsTotal = results.Count();
            var data = results.Skip(tableModel.skip).Take(tableModel.pageSize).ToList();

            roleListViewModel.draw = tableModel.draw;
            roleListViewModel.recordsFiltered = tableModel.recordsTotal;
            roleListViewModel.recordsTotal = tableModel.recordsTotal;
            roleListViewModel.data = data;

            return roleListViewModel;
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
