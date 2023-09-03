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
    public class LogLoginService : ILogLoginService
    {
        private readonly ILogLoginRepository _logLoginRepository;
        public LogLoginService(ILogLoginRepository logLoginRepository) 
        { 
            _logLoginRepository = logLoginRepository;
        }

        public async Task<IEnumerable<LogLogin>> GetAll(string par = " 1=1 ")
        {
            return await _logLoginRepository.GetAll(par);
        }

        public async Task<DataTableResponseModel<object>> GetByFilterModel(DataTablePostModel model)
        {
            try
            {
                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].name;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _logLoginRepository.GetByFilterModel(filterData);
                var dataCount = await _logLoginRepository.GetCount(filterData);

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

        public async Task<LogLogin> GetById(Guid id)
        {
            return await _logLoginRepository.GetById(id);
        }

        public async Task<int> Insert(LogLogin model)
        {
            return await _logLoginRepository.Insert(model);
        }
    }
}
