using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
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
    public class ArchiveOwnerService : IArchiveOwnerService
    {
        private readonly IArchiveOwnerRepository _archiveOwnerRepository;

        public ArchiveOwnerService(IArchiveOwnerRepository archiveOwnerRepository) => _archiveOwnerRepository = archiveOwnerRepository;
        public async Task<int> Delete(MstArchiveOwner model)
        {
            return await _archiveOwnerRepository.Delete(model);
        }

        public async Task<IEnumerable<MstArchiveOwner>> GetAll()
        {
            return await _archiveOwnerRepository.GetAll();
        }

        public async Task<IEnumerable<MstArchiveOwner>> GetById(Guid id)
        {
            var result = await _archiveOwnerRepository.GetById(id);
            return result;
        }

        public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
        {
            try
            {
                var filterData = new DataTableModel
                {
                    sortColumn = model.columns[model.order[0].column].name,
                    sortColumnDirection = model.order[0].dir,
                    searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                    pageSize = model.length,
                    skip = model.start
                };

                var results = await _archiveOwnerRepository.GetByFilterModel(filterData);
                var dataCount = await _archiveOwnerRepository.GetCountByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<object>
                {
                    draw = model.draw,
                    recordsTotal = dataCount,
                    recordsFiltered = dataCount,
                    data = results.ToList()
                };

                return responseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> Insert(MstArchiveOwner model)
        {
            return await _archiveOwnerRepository.Insert(model);
        }

        public async Task<bool> InsertBulk(List<MstArchiveOwner> MstArchiveOwners)
        {
            return await _archiveOwnerRepository.InsertBulk(MstArchiveOwners);
        }

        public async Task<int> Update(MstArchiveOwner model)
        {
            return await _archiveOwnerRepository.Update(model);
        }
    }
}
