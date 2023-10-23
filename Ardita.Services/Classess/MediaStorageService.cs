using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Web.WebPages;

namespace Ardita.Services.Classess;

public class MediaStorageService : IMediaStorageService
{
    private readonly IMediaStorageRepository _mediaStorageRepository;

    public MediaStorageService(IMediaStorageRepository mediaStorageRepository)
    {
        _mediaStorageRepository = mediaStorageRepository;
    }

    public async Task<int> Delete(TrxMediaStorage model) => await _mediaStorageRepository.Delete(model);

    public async Task<IEnumerable<TrxMediaStorage>> GetAll(string par = " 1=1 ") => await _mediaStorageRepository.GetAll(par);
    public async Task<int> GetCount(string par = " 1=1 ") => await _mediaStorageRepository.GetCount(par);
    public async Task<int> GetCountByRackLits(bool isActive) => await _mediaStorageRepository.GetCountByRackList(isActive);

    public async Task<TrxMediaStorage> GetById(Guid id)
    {
        return await _mediaStorageRepository.GetById(id);
    }
    public async Task<TrxMediaStorageDetail> GetDetailByArchiveId(Guid id)
    {
        return await _mediaStorageRepository.GetDetailByArchiveId(id);
    }

    public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
    {
        try
        {
            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].name,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start,
                SessionUser = model.SessionUser,
                advanceSearch = new SearchModel
                {
                    Search = model.columns[2].search.value == null ? "1=1" : model.columns[2].search.value
                }
            };

            var dataCount = await _mediaStorageRepository.GetCountByFilterModel(filterData);
            var results = await _mediaStorageRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<object>
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
    public async Task<int> UpdateDetail(TrxMediaStorageDetail model)
    {
        return await _mediaStorageRepository.UpdateDetail(model);
    }

    public async Task<bool> UpdateDetailIsUsed(Guid archiveId, string usedBy, bool isUsed)
    {
        return await _mediaStorageRepository.UpdateDetailIsUsed(archiveId, usedBy, isUsed);
    }
}
