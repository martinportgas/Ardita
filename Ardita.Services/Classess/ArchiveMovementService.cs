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
    public class ArchiveMovementService : IArchiveMovementService
    {
        private readonly IArchiveMovementRepository _archiveMovementRepository;
        private readonly IArchiveMovementDetailRepository _archiveMovementDetailRepository;
        public ArchiveMovementService(IArchiveMovementRepository archiveMovementRepository,
            IArchiveMovementDetailRepository archiveMovementDetailRepository)
        {
            _archiveMovementRepository = archiveMovementRepository;
            _archiveMovementDetailRepository = archiveMovementDetailRepository;
        }
        public async Task<int> Delete(TrxArchiveMovement model)
        {
            return await _archiveMovementRepository.Delete(model);
        }

        public async Task<IEnumerable<TrxArchiveMovement>> GetAll()
        {
            return await _archiveMovementRepository.GetAll();
        }

        public async Task<TrxArchiveMovement> GetById(Guid id)
        {
            return await _archiveMovementRepository.GetById(id);
        }

        public async Task<int> Insert(TrxArchiveMovement model)
        {
            return await _archiveMovementRepository.Insert(model);
        }
        public async Task<bool> InsertBulk(List<TrxArchiveMovement> models)
        {
            return await _archiveMovementRepository.InsertBulk(models);
        }

        public async Task<int> Update(TrxArchiveMovement model)
        {
            return await _archiveMovementRepository.Update(model);
        }
        public async Task<DataTableResponseModel<TrxArchiveMovement>> GetList(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _archiveMovementRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _archiveMovementRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<TrxArchiveMovement>();

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

        public async Task<IEnumerable<TrxArchiveMovementDetail>> GetDetailByMainId(Guid id)
        {
            return await _archiveMovementDetailRepository.GetByMainId(id);
        }

        public async Task<IEnumerable<TrxArchiveMovementDetail>> GetDetailAll()
        {
            return await _archiveMovementDetailRepository.GetAll();
        }

        public async Task<bool> InsertBulkDetail(List<TrxArchiveMovementDetail> models)
        {
            return await _archiveMovementDetailRepository.InsertBulk(models);
        }

        public async Task<int> DeleteDetailByMainId(Guid Id)
        {
            return await _archiveMovementDetailRepository.DeleteByMainId(Id);
        }

        public async Task<int> InsertDetail(TrxArchiveMovementDetail model)
        {
            return await _archiveMovementDetailRepository.Insert(model);
        }

        public async Task<int> Submit(TrxArchiveMovement model)
        {
            return await _archiveMovementRepository.Submit(model);
        }
    }
}
