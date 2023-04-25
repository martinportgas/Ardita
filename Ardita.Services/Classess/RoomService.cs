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
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<int> Delete(TrxRoom model)
        {
            return await _roomRepository.Delete(model);
        }

        public async Task<IEnumerable<TrxRoom>> GetAll()
        {
            return await _roomRepository.GetAll();
        }

        public async Task<IEnumerable<TrxRoom>> GetById(Guid id)
        {
            return await _roomRepository.GetById(id);
        }

        public async Task<int> Insert(TrxRoom model)
        {
            return await _roomRepository.Insert(model);
        }

        public async Task<int> Update(TrxRoom model)
        {
            return await _roomRepository.Update(model);
        }
        public async Task<DataTableResponseModel<TrxRoom>> GetListClassification(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _roomRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _roomRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<TrxRoom>();

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
        public async Task<bool> InsertBulk(List<TrxRoom> rooms)
        {
            return await _roomRepository.InsertBulk(rooms);
        }
    }
}
