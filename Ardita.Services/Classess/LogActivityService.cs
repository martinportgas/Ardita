﻿using Ardita.Models.DbModels;
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
    public class LogActivityService : ILogActivityService
    {
        private readonly ILogActivityRepository _logActivityRepository;
        public LogActivityService(ILogActivityRepository logActivityRepository) 
        { 
            _logActivityRepository = logActivityRepository;
        }

        public async Task<IEnumerable<LogActivity>> GetAll()
        {
            return await _logActivityRepository.GetAll();
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

                var results = await _logActivityRepository.GetByFilterModel(filterData);
                var dataCount = await _logActivityRepository.GetCount(filterData);

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

        public async Task<LogActivity> GetById(Guid id)
        {
            return await _logActivityRepository.GetById(id);
        }

        public async Task<int> Insert(LogActivity model)
        {
            return await _logActivityRepository.Insert(model);
        }
    }
}
