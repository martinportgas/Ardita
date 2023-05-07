﻿using Ardita.Models.DbModels;
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

        public async Task<IEnumerable<TrxArchiveDestroy>> GetAll()
        {
            return await _archiveDestroyRepository.GetAll();
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
        public async Task<DataTableResponseModel<TrxArchiveDestroy>> GetList(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _archiveDestroyRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _archiveDestroyRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<TrxArchiveDestroy>();

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