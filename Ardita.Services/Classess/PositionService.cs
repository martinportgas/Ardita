using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Positions;
using Ardita.Models.ViewModels.Users;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<PositionListViewModel> GetListPosition(DataTableModel tableModel)
        {
            var positionListViewModel = new PositionListViewModel();

            var positionResult = await _positionRepository.GetAll();
            var results = (from position in positionResult
                          select new PositionListViewDetailModel 
                          { 
                            PositionId = position.PosittionId,
                            PositionCode = position.Code,
                            PositionName = position.Name,
                            IsActive = position.IsActive
                          });

            if (!string.IsNullOrEmpty(tableModel.searchValue))
            {
                results = results.Where(
                    x => x.PositionCode.Contains(tableModel.searchValue)
                    || x.PositionName.Contains(tableModel.searchValue)
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
