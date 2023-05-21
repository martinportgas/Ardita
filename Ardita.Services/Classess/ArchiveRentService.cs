using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class ArchiveRentService : IArchiveRentService
    {
        private readonly IArchiveRentRepository _archiveRentRepository;
        public ArchiveRentService(IArchiveRentRepository archiveRentRepository)
        {
            _archiveRentRepository = archiveRentRepository;
        }
        public Task<int> Delete(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrxArchiveRent>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
        {
            try
            {
                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].name;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;
                filterData.IsArchiveActive = model.IsArchiveActive;

                var dataCount = await _archiveRentRepository.GetCountByFilterModel(filterData);
                var results = await _archiveRentRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<object>();

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

        public Task<IEnumerable<TrxArchiveRent>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountByFilterModel(DataTableModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Insert(TrxArchiveRent model)
        {
            return await _archiveRentRepository.Insert(model);
        }

        public Task<int> Update(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }
    }
}
