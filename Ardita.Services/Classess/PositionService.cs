using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Positions;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess
{
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        public PositionService(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<int> Delete(MstPosition model)
        {
            return await _positionRepository.Delete(model);
        }

        public async Task<IEnumerable<MstPosition>> GetAll()
        {
            return await _positionRepository.GetAll();
        }

        public async Task<MstPosition> GetById(Guid id)
        {
            return await _positionRepository.GetById(id);
        }

        public async Task<DataTableResponseModel<MstPosition>> GetListPositions(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _positionRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _positionRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<MstPosition>();

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

        public async Task<int> Insert(MstPosition model)
        {
            return await _positionRepository.Insert(model);
        }

        public async Task<int> Update(MstPosition model)
        {
            return await _positionRepository.Update(model);
        }
    }
}
