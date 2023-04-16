using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Positions;
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

        public async Task<IEnumerable<MstPosition>> GetById(Guid id)
        {
            return await _positionRepository.GetById(id);
        }
        public async Task<MstPosition> GetFirstById(Guid id)
        {
            var result = await _positionRepository.GetById(id);
            return result.FirstOrDefault();
        }

        public async Task<PositionListViewModel> GetListPosition(DataTableModel tableModel)
        {
            var positionListViewModel = new PositionListViewModel();

            var positionResult = await _positionRepository.GetAll();
            var results = (from position in positionResult
                           select new PositionListViewDetailModel
                           {
                               PositionId = position.PositionId,
                               PositionCode = position.Code,
                               PositionName = position.Name,
                               IsActive = position.IsActive
                           });
            //if (!(string.IsNullOrEmpty(tableModel.sortColumn) && string.IsNullOrEmpty(tableModel.sortColumnDirection)))
            //{
            //    results = results.OrderBy(tableModel.sortColumn + " " + tableModel.sortColumnDirection);
            //}

            if (!string.IsNullOrEmpty(tableModel.searchValue))
            {
                results = results.Where(
                    x => x.PositionCode.ToUpper().Contains(tableModel.searchValue.ToUpper())
                    || x.PositionName.ToUpper().Contains(tableModel.searchValue.ToUpper())
                );
            }
            tableModel.recordsTotal = results.Count();
            var data = results.Skip(tableModel.skip).Take(tableModel.pageSize).ToList();

            positionListViewModel.draw = tableModel.draw;
            positionListViewModel.recordsFiltered = tableModel.recordsTotal;
            positionListViewModel.recordsTotal = tableModel.recordsTotal;
            positionListViewModel.data = data;

            return positionListViewModel;
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
