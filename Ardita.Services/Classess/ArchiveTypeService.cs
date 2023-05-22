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
    public class ArchiveTypeService : IArchiveTypeService
    {
        private readonly IArchiveTypeRepository _archiveTypeRepository;
        public ArchiveTypeService(IArchiveTypeRepository archiveTypeRepository) => _archiveTypeRepository = archiveTypeRepository;
        public async Task<int> Delete(MstArchiveType model)
        {
            return await _archiveTypeRepository.Delete(model);
        }

        public async Task<IEnumerable<MstArchiveType>> GetAll()
        {
            return await _archiveTypeRepository.GetAll();
        }

        public async Task<IEnumerable<MstArchiveType>> GetById(Guid id)
        {
            return await _archiveTypeRepository.GetById(id);
        }

        public async Task<DataTableResponseModel<MstArchiveType>> GetList(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _archiveTypeRepository.GetCount();

                var filterData = new DataTableModel
                {
                    sortColumn = model.columns[model.order[0].column].data,
                    sortColumnDirection = model.order[0].dir,
                    searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                    pageSize = model.length,
                    skip = model.start
                };

                var results = await _archiveTypeRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<MstArchiveType>
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

        public async Task<int> Insert(MstArchiveType model)
        {
            return await _archiveTypeRepository.Insert(model);
        }

        public async Task<bool> InsertBulk(List<MstArchiveType> MstArchiveTypes)
        {
            return await _archiveTypeRepository.InsertBulk(MstArchiveTypes);
        }

        public async Task<int> Update(MstArchiveType model)
        {
            return await _archiveTypeRepository.Update(model);
        }
    }
}
