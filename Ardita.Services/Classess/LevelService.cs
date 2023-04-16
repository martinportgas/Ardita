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

        public async Task<IEnumerable<TrxLevel>> GetAll()
        {
            return await _levelRepository.GetAll();
        }

        public async Task<IEnumerable<TrxLevel>> GetById(Guid id)
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
        public async Task<DataTableResponseModel<TrxLevel>> GetListClassification(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _levelRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _levelRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<TrxLevel>();

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
    }
}
