using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.Positions;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Azure.Core;

namespace Ardita.Services.Classess
{
    public class ClassificationService : IClassificationService
    {
        private readonly IClassificationRepository _classificationRepository;
        public ClassificationService(IClassificationRepository classificationRepository)
        {
            _classificationRepository = classificationRepository;
        }
        public async Task<int> Delete(TrxClassification model)
        {
            return await _classificationRepository.Delete(model);
        }

        public async Task<IEnumerable<TrxClassification>> GetAll()
        {
            return await _classificationRepository.GetAll();
        }

        public async Task<IEnumerable<TrxClassification>> GetById(Guid id)
        {
            return await _classificationRepository.GetById(id);
        }
        public async Task<IEnumerable<TrxClassification>> GetByTypeId(Guid id)
        {
            var result = await _classificationRepository.GetAll();
            result = result.Where(x => x.TypeClassificationId == id);
            return result;
        }

        public async Task<int> Insert(TrxClassification model)
        {
            return await _classificationRepository.Insert(model);
        }
        public async Task<bool> InsertBulk(List<TrxClassification> models)
        {
            return await _classificationRepository.InsertBulk(models);
        }
        public async Task<int> Update(TrxClassification model)
        {
            return await _classificationRepository.Update(model);
        }
        public async Task<DataTableResponseModel<TrxClassification>> GetListClassification(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _classificationRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _classificationRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<TrxClassification>();

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
