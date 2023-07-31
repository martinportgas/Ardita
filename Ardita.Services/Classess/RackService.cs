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
    public class RackService : IRackService
    {
        private readonly IRackRepository _rackRepository;
        public RackService(IRackRepository rackRepository)
        {
            _rackRepository = rackRepository;
        }
        public async Task<int> Delete(TrxRack model)
        {
            return await _rackRepository.Delete(model);
        }

        public async Task<IEnumerable<TrxRack>> GetAll()
        {
            return await _rackRepository.GetAll();
        }

        public async Task<TrxRack> GetById(Guid id)
        {
            return await _rackRepository.GetById(id);
        }

        public async Task<int> Insert(TrxRack model)
        {
            return await _rackRepository.Insert(model);
        }

        public async Task<int> Update(TrxRack model)
        {
            return await _rackRepository.Update(model);
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

                var results = await _rackRepository.GetByFilterModel(filterData);
                var dataCount = await _rackRepository.GetCount(filterData);

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

        public async Task<bool> InsertBulk(List<TrxRack> racks)
        {
            return await _rackRepository.InsertBulk(racks);
        }
    }
}
