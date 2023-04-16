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
    public class ClassificationSubjectService : IClassificationSubjectService
    {
        private readonly IClassificationSubjectRepository _classificationSubjectRepository;
        public ClassificationSubjectService(IClassificationSubjectRepository classificationSubjectRepository)
        {
            _classificationSubjectRepository = classificationSubjectRepository;
        }
        public async Task<int> Delete(TrxSubjectClassification model)
        {
            return await _classificationSubjectRepository.Delete(model);
        }

        public async Task<IEnumerable<TrxSubjectClassification>> GetAll()
        {
            return await _classificationSubjectRepository.GetAll();
        }

        public async Task<IEnumerable<TrxSubjectClassification>> GetById(Guid id)
        {
            return await _classificationSubjectRepository.GetById(id);
        }
        public async Task<IEnumerable<TrxSubjectClassification>> GetByClassificationId(Guid id)
        {
            var result = await _classificationSubjectRepository.GetAll();
            result = result.Where(x => x.ClassificationId == id);
            return result;
        }

        public async Task<int> Insert(TrxSubjectClassification model)
        {
            return await _classificationSubjectRepository.Insert(model);
        }

        public async Task<int> Update(TrxSubjectClassification model)
        {
            return await _classificationSubjectRepository.Update(model);
        }
        public async Task<DataTableResponseModel<TrxSubjectClassification>> GetListClassificationSubject(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _classificationSubjectRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _classificationSubjectRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<TrxSubjectClassification>();

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
