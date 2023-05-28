using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class ArchiveRentService : IArchiveRentService
    {
        private readonly IArchiveRentRepository _archiveRentRepository;
        public ArchiveRentService(IArchiveRentRepository archiveRentRepository)
        {
            _archiveRentRepository = archiveRentRepository;
        }
        public Task<int> Delete(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrxArchiveRent>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
        {
            try
            {
                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].name;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;
                filterData.IsArchiveActive = model.IsArchiveActive;

                var dataCount = await _archiveRentRepository.GetCountByFilterModel(filterData);
                var results = await _archiveRentRepository.GetByFilterModel(filterData);

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

        public async Task<IEnumerable<TrxArchiveRent>> GetById(Guid id)
        {
            return await _archiveRentRepository.GetById(id);
        }

        public async Task<int> Insert(TrxArchiveRent model)
        {
            return await _archiveRentRepository.Insert(model);
        }

        public Task<int> Update(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataTableResponseModel<object>> GetApprovalList(DataTablePostModel model)
        {
            try
            {
                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].name;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;
                filterData.IsArchiveActive = model.IsArchiveActive;

                var dataCount = await _archiveRentRepository.GetApprovalCountByFilterModel(filterData);
                var results = await _archiveRentRepository.GetApprovalByFilterModel(filterData);

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

        public async Task<int> Approval(Guid id, string description, int status, Guid User)
        {
            return await _archiveRentRepository.Approval(id, description, status, User);
        }

        public async Task<DataTableResponseModel<object>> GetRetrievalList(DataTablePostModel model)
        {
            try
            {
                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].name;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;
                filterData.IsArchiveActive = model.IsArchiveActive;

                var dataCount = await _archiveRentRepository.GetRetrievalCountByFilterModel(filterData);
                var results = await _archiveRentRepository.GetRetrievalByFilterModel(filterData);

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

        public async Task<DataTableResponseModel<object>> GetReturnList(DataTablePostModel model)
        {
            try
            {
                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].name;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;
                filterData.IsArchiveActive = model.IsArchiveActive;

                var dataCount = await _archiveRentRepository.GetReturnCountByFilterModel(filterData);
                var results = await _archiveRentRepository.GetReturnByFilterModel(filterData);

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

        public async Task<IEnumerable<object>> GetRetrievalByArchiveRentId(Guid id, string form)
        {
            return await _archiveRentRepository.GetRetrievalByArchiveRentId(id, form);
        }

        public async Task<bool> ValidateQRBoxWithArchiveRentId(Guid ArchiveRentId, string mediaInActiveCode)
        {
            return await _archiveRentRepository.ValidateQRBoxWithArchiveRentId(ArchiveRentId, mediaInActiveCode);
        }

        public async Task<IEnumerable<object>> GetRetrievalDetailByArchiveRentId(Guid ArchiveId, int sort)
        {
            return await _archiveRentRepository.GetRetrievalDetailByArchiveRentId(ArchiveId, sort);
        }

        public async Task<bool> UpdateArchiveRent(Guid ArchiveRentId, Guid UserId)
        {
            return await _archiveRentRepository.UpdateArchiveRent(ArchiveRentId, UserId);
        }
    }
}
