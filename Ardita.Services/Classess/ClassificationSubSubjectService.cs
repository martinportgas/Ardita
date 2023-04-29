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
    public class ClassificationSubSubjectService : IClassificationSubSubjectService
    {
        private readonly IClassificationSubSubjectRepository _classificationSubSubjectRepository;
        private readonly IClassificationPermissionRepository _classificationPermissionRepository;
        private readonly IPositionRepository _positionRepository;
        public ClassificationSubSubjectService(
            IClassificationSubSubjectRepository classificationSubSubjectRepository,
            IClassificationPermissionRepository classificationPermissionRepository,
            IPositionRepository  positionRepository)
        {
            _classificationSubSubjectRepository = classificationSubSubjectRepository;
            _classificationPermissionRepository = classificationPermissionRepository;
            _positionRepository = positionRepository;
        }
        public async Task<int> Delete(TrxSubSubjectClassification model)
        {
            return await _classificationSubSubjectRepository.Delete(model);
        }
        public async Task<int> DeleteDetail(Guid id)
        {
            return await _classificationPermissionRepository.DeleteByMainId(id);
        }

        public async Task<IEnumerable<TrxSubSubjectClassification>> GetAll()
        {
            return await _classificationSubSubjectRepository.GetAll();
        }

        public async Task<IEnumerable<TrxSubSubjectClassification>> GetById(Guid id)
        {
            return await _classificationSubSubjectRepository.GetById(id);
        }
        public async Task<IEnumerable<TrxPermissionClassification>> GetDetailByMainId(Guid id)
        {
            return await _classificationPermissionRepository.GetByMainId(id);
        }

        public async Task<int> Insert(TrxSubSubjectClassification model)
        {
            return await _classificationSubSubjectRepository.Insert(model);
        }
        public async Task<bool> InsertBulk(List<TrxSubSubjectClassification> models)
        {
            return await _classificationSubSubjectRepository.InsertBulk(models);
        }
        public async Task<int> InsertDetail(TrxPermissionClassification model)
        {
            return await _classificationPermissionRepository.Insert(model);
        }
        public async Task<int> Update(TrxSubSubjectClassification model)
        {
            return await _classificationSubSubjectRepository.Update(model);
        }
        public async Task<DataTableResponseModel<TrxSubSubjectClassification>> GetListClassificationSubSubject(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _classificationSubSubjectRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _classificationSubSubjectRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<TrxSubSubjectClassification>();

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
        public async Task<IEnumerable<TrxPermissionClassification>> GetListDetailPermissionClassifications(Guid id)
        {
            var positionData = await _positionRepository.GetAll();
            var subDetail = await _classificationPermissionRepository.GetByMainId(id);

            var result = (from detail in subDetail
                                 join position in positionData on detail.PositionId equals position.PositionId
                                 select new TrxPermissionClassification
                                 {
                                     PositionId = detail.PositionId,
                                     Position = position,
                                 }).ToList();

            return result;
        }
    }
}
