using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Report;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace Ardita.Services.Classess
{
    public class ArchiveDestroyService : IArchiveDestroyService
    {
        private readonly IArchiveDestroyRepository _archiveDestroyRepository;
        private readonly IArchiveDestroyDetailRepository _archiveDestroyDetailRepository;
        public ArchiveDestroyService(IArchiveDestroyRepository archiveDestroyRepository,
            IArchiveDestroyDetailRepository archiveDestroyDetailRepository)
        {
            _archiveDestroyRepository = archiveDestroyRepository;
            _archiveDestroyDetailRepository = archiveDestroyDetailRepository;
        }
        public async Task<int> Delete(TrxArchiveDestroy model)
        {
            return await _archiveDestroyRepository.Delete(model);
        }

        public async Task<IEnumerable<TrxArchiveDestroy>> GetAll(string par = " 1=1 ")
        {
            return await _archiveDestroyRepository.GetAll(par);
        }

        public async Task<TrxArchiveDestroy> GetById(Guid id)
        {
            return await _archiveDestroyRepository.GetById(id);
        }

        public async Task<int> Insert(TrxArchiveDestroy model)
        {
            return await _archiveDestroyRepository.Insert(model);
        }
        public async Task<bool> InsertBulk(List<TrxArchiveDestroy> models)
        {
            return await _archiveDestroyRepository.InsertBulk(models);
        }

        public async Task<int> Update(TrxArchiveDestroy model)
        {
            return await _archiveDestroyRepository.Update(model);
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
                filterData.advanceSearch = new SearchModel
                {
                    Search = model.columns[2].search.value == null ? "1=1" : model.columns[2].search.value
                };
                filterData.SessionUser = model.SessionUser;

                var dataCount = await _archiveDestroyRepository.GetCountByFilterModel(filterData);
                var results = await _archiveDestroyRepository.GetByFilterModel(filterData);

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

        public async Task<IEnumerable<TrxArchiveDestroyDetail>> GetDetailByMainId(Guid id)
        {
            return await _archiveDestroyDetailRepository.GetByMainId(id);
        }

        public async Task<IEnumerable<TrxArchiveDestroyDetail>> GetDetailAll()
        {
            return await _archiveDestroyDetailRepository.GetAll();
        }

        public async Task<bool> InsertBulkDetail(List<TrxArchiveDestroyDetail> models)
        {
            return await _archiveDestroyDetailRepository.InsertBulk(models);
        }

        public async Task<int> DeleteDetailByMainId(Guid Id)
        {
            return await _archiveDestroyDetailRepository.DeleteByMainId(Id);
        }

        public async Task<int> InsertDetail(TrxArchiveDestroyDetail model)
        {
            return await _archiveDestroyDetailRepository.Insert(model);
        }

        public async Task<int> Submit(TrxArchiveDestroy model)
        {
            return await _archiveDestroyRepository.Submit(model);
        }
    }
}
