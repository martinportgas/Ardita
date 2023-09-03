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
    public class ClassificationTypeService : IClassificationTypeService
    {
        private readonly IClassificationTypeRepository _classificationTypeRepository;
        public ClassificationTypeService(IClassificationTypeRepository classificationTypeRepository)
        {
            _classificationTypeRepository = classificationTypeRepository;
        }
        public async Task<int> Delete(MstTypeClassification model)
        {
            return await _classificationTypeRepository.Delete(model);
        }

        public async Task<IEnumerable<MstTypeClassification>> GetAll(string par = " 1=1 ")
        {
            return await _classificationTypeRepository.GetAll(par);
        }

        public async Task<MstTypeClassification> GetById(Guid id)
        {
            return await _classificationTypeRepository.GetById(id);
        }

        public async Task<int> Insert(MstTypeClassification model)
        {
            return await _classificationTypeRepository.Insert(model);
        }
        public async Task<bool> InsertBulk(List<MstTypeClassification> models)
        {
            return await _classificationTypeRepository.InsertBulk(models);
        }

        public async Task<int> Update(MstTypeClassification model)
        {
            return await _classificationTypeRepository.Update(model);
        }
        public async Task<DataTableResponseModel<object>> GetListClassificationType(DataTablePostModel model)
        {
            try
            {
                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].name;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _classificationTypeRepository.GetByFilterModel(filterData);
                var dataCount = await _classificationTypeRepository.GetCountByFilterModel(filterData);

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
    }
}
