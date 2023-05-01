﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class MediaStorageService : IMediaStorageService
{
    private readonly IMediaStorageRepository _mediaStorageRepository;

    public MediaStorageService(IMediaStorageRepository mediaStorageRepository) => _mediaStorageRepository = mediaStorageRepository;

    public async Task<int> Delete(TrxMediaStorage model) => await _mediaStorageRepository.Delete(model);

    public async Task<IEnumerable<TrxMediaStorage>> GetAll() => await _mediaStorageRepository.GetAll();

    public async Task<TrxMediaStorage> GetById(Guid id)
    {
        return await _mediaStorageRepository.GetById(id);
    }

    public async Task<DataTableResponseModel<TrxMediaStorage>> GetList(DataTablePostModel model)
    {
        try
        {
            var dataCount = await _mediaStorageRepository.GetCount();

            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].data,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start
            };

            var results = await _mediaStorageRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<TrxMediaStorage>
            {
                draw = model.draw,
                recordsTotal = dataCount,
                recordsFiltered = dataCount,
                data = results.ToList()
            };

            return responseModel;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<int> Insert(TrxMediaStorage model, string[] archiveId)
    {
        List<TrxMediaStorageDetail> trxMediaStorageDetail = new();
        Guid ArchiveId;
        foreach (var item in archiveId)
        {
            TrxMediaStorageDetail trxMediaStorageDetailTemp = new();

            ArchiveId = new Guid(item);
            trxMediaStorageDetailTemp.ArchiveId = ArchiveId;
            trxMediaStorageDetailTemp.CreatedDate = model.CreatedDate;
            trxMediaStorageDetailTemp.CreatedBy = model.CreatedBy;

            trxMediaStorageDetail.Add(trxMediaStorageDetailTemp);
        }

        return await _mediaStorageRepository.Insert(model, trxMediaStorageDetail);
    }

    public async Task<int> Update(TrxMediaStorage model, string[] archiveId)
    {
        List<TrxMediaStorageDetail> trxMediaStorageDetail = new();
        Guid ArchiveId;
        foreach (var item in archiveId)
        {
            TrxMediaStorageDetail trxMediaStorageDetailTemp = new();

            ArchiveId = new Guid(item);
            trxMediaStorageDetailTemp.ArchiveId = ArchiveId;
          

            trxMediaStorageDetail.Add(trxMediaStorageDetailTemp);
        }

        return await _mediaStorageRepository.Update(model, trxMediaStorageDetail);
    }
}
