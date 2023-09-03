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
    public class LevelService : ILevelService
    {
        private readonly ILevelRepository _levelRepository;
        public LevelService(ILevelRepository levelRepository)
        {
            _levelRepository = levelRepository;
        }
        public async Task<int> Delete(TrxLevel model)
        {
            return await _levelRepository.Delete(model);
        }

        public async Task<IEnumerable<TrxLevel>> GetAll(string par = " 1=1 ")
        {
            return await _levelRepository.GetAll(par);
        }

        public async Task<TrxLevel> GetById(Guid id)
        {
            return await _levelRepository.GetById(id);
        }

        public async Task<int> Insert(TrxLevel model)
        {
            return await _levelRepository.Insert(model);
        }

        public async Task<int> Update(TrxLevel model)
        {
            return await _levelRepository.Update(model);
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

                var results = await _levelRepository.GetByFilterModel(filterData);
                var dataCount = await _levelRepository.GetCount(filterData);
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

        public async Task<bool> InsertBulk(List<TrxLevel> levels)
        {
            return await _levelRepository.InsertBulk(levels);
        }
    }
}
