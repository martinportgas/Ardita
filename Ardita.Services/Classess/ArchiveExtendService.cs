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
    public class ArchiveExtendService : IArchiveExtendService
    {
        private readonly IArchiveExtendRepository _archiveExtendRepository;
        private readonly IArchiveExtendDetailRepository _archiveExtendDetailRepository;
        public ArchiveExtendService(IArchiveExtendRepository archiveExtendRepository, IArchiveExtendDetailRepository archiveExtendDetailRepository)
        {
            _archiveExtendRepository = archiveExtendRepository;
            _archiveExtendDetailRepository = archiveExtendDetailRepository;
        }
        public async Task<int> Delete(TrxArchiveExtend model)
        {
            return await _archiveExtendRepository.Delete(model);
        }
        public async Task<int> DeleteDetailByMainId(Guid Id)
        {
            return await _archiveExtendDetailRepository.DeleteByMainId(Id);
        }

        public async Task<IEnumerable<TrxArchiveExtend>> GetAll()
        {
            return await _archiveExtendRepository.GetAll();
        }

        public async Task<IEnumerable<TrxArchiveExtend>> GetById(Guid id)
        {
            return await _archiveExtendRepository.GetById(id);
        }
        public async Task<IEnumerable<TrxArchiveExtendDetail>> GetDetailByMainId(Guid id)
        {
            return await _archiveExtendDetailRepository.GetByMainId(id);
        }

        public async Task<int> Insert(TrxArchiveExtend model)
        {
            return await _archiveExtendRepository.Insert(model);
        }
        public async Task<bool> InsertBulk(List<TrxArchiveExtend> models)
        {
            return await _archiveExtendRepository.InsertBulk(models);
        }
        public async Task<bool> InsertBulkDetail(List<TrxArchiveExtendDetail> models)
        {
            return await _archiveExtendDetailRepository.InsertBulk(models);
        }

        public async Task<int> Update(TrxArchiveExtend model)
        {
            return await _archiveExtendRepository.Update(model);
        }
        public async Task<DataTableResponseModel<TrxArchiveExtend>> GetList(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _archiveExtendRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _archiveExtendRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<TrxArchiveExtend>();

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
    }
}
