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
    public class ArchiveRentService : IArchiveRentService
    {
        private readonly IArchiveRentRepository _archiveRentRepository;
        private readonly IMediaStorageInActiveRepository _mediaStorageInActiveRepository;
        public ArchiveRentService(IArchiveRentRepository archiveRentRepository, IMediaStorageInActiveRepository mediaStorageInActiveRepository)
        {
            _archiveRentRepository = archiveRentRepository;
            _mediaStorageInActiveRepository = mediaStorageInActiveRepository;
        }
        public Task<int> Delete(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TrxArchiveRent>> GetAll()
        {
            return await _archiveRentRepository.GetAll();
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

        public async Task<TrxArchiveRent> GetById(Guid id)
        {
            return await _archiveRentRepository.GetById(id);
        }

        public async Task<int> Insert(TrxArchiveRent model, MstBorrower borrower, string[] listArchive)
        {
            var listDetail = await GetDetail(model, listArchive);
            return await _archiveRentRepository.Insert(model, borrower, listDetail);
        }

        public async Task<int> Update(TrxArchiveRent model, MstBorrower borrower, string[] listArchive)
        {
            var listDetail = await GetDetail(model, listArchive);
            return await _archiveRentRepository.Update(model, borrower, listDetail);
        }
        private async Task<List<TrxArchiveRentDetail>> GetDetail(TrxArchiveRent model, string[] listArchive)
        {
            TrxArchiveRentDetail item;
            List<TrxArchiveRentDetail> list = new();
            if(listArchive.Length > 0)
            {
                foreach (string archive in listArchive)
                {
                    Guid ArchiveId = Guid.Empty;
                    if (Guid.TryParse(archive, out ArchiveId))
                    {
                        var detail = await _mediaStorageInActiveRepository.GetDetailByArchiveId(ArchiveId);

                        if (detail != null)
                        {
                            item = new();
                            item.TrxArchiveRentId = model.TrxArchiveRentId;
                            item.ArchiveId = detail.ArchiveId;
                            item.MediaStorageInActiveId = detail.MediaStorageInActiveId;
                            item.Sort = detail.Sort;
                            item.CreatedBy = model.CreatedBy;
                            item.CreatedDate = model.CreatedDate;

                            list.Add(item);
                        }
                    }

                }
            }
            return list;
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
        public async Task<IEnumerable<object>> GetReturnByArchiveRentId(Guid id, string form)
        {
            return await _archiveRentRepository.GetReturnByArchiveRentId(id, form);
        }
        public async Task<IEnumerable<object>> GetReturnDetailByArchiveRentId(Guid ArchiveId, int sort)
        {
            return await _archiveRentRepository.GetReturnDetailByArchiveRentId(ArchiveId, sort);
        }

        public async Task<IEnumerable<MstBorrower>> GetBorrower()
        {
            return await _archiveRentRepository.GetBorrower();
        }

        public async Task<IEnumerable<object>> GetByBorrowerId(Guid Id)
        {
            return await _archiveRentRepository.GetByBorrowerId(Id);
        }

        public async Task<IEnumerable<VwArchiveRentBox>> GetArchiveRentBoxById(Guid Id)
        {
            return await _archiveRentRepository.GetArchiveRentBoxById(Id);
        }
    }
}
