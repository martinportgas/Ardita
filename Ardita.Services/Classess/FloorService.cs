using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepository;
        public FloorService(IFloorRepository floorRepository)
        {
            _floorRepository = floorRepository;
        }

        public async Task<int> Delete(TrxFloor model)
        {
            return await _floorRepository.Delete(model);
        }

        public async Task<IEnumerable<TrxFloor>> GetAll()
        {
            return await _floorRepository.GetAll();
        }

        public async Task<IEnumerable<TrxFloor>> GetById(Guid id)
        {
            return await _floorRepository.GetById(id);
        }

        public async Task<int> Insert(TrxFloor model)
        {
            return await _floorRepository.Insert(model);
        }

        public async Task<int> Update(TrxFloor model)
        {
            return await _floorRepository.Update(model);
        }
        public async Task<DataTableResponseModel<TrxFloor>> GetListClassification(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _floorRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _floorRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<TrxFloor>();

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

        public async Task<bool> InsertBulk(List<TrxFloor> floors)
        {
            return await _floorRepository.InsertBulk(floors);
        }
    }
}
