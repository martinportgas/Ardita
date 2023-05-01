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

        public async Task<MstRole> GetById(Guid id)
        {
            return await _roleRepository.GetById(id);
        }

    

        public async Task<DataTableResponseModel<MstRole>> GetListRoles(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _roleRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _roleRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<MstRole>();

                responseModel.draw = model.draw;
                responseModel.recordsTotal = dataCount;
                responseModel.recordsFiltered = dataCount;
                responseModel.data = results.ToList();

                return responseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
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
