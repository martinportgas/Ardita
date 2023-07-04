using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ardita.Services.Classess;

public class MediaStorageInActiveService : IMediaStorageInActiveService
{
    private readonly IMediaStorageInActiveRepository _mediaStorageInActiveRepository;

    public MediaStorageInActiveService(IMediaStorageInActiveRepository mediaStorageInActiveRepository)
    {
        _mediaStorageInActiveRepository = mediaStorageInActiveRepository;
    }

    public async Task<IEnumerable<object>> GetDetailArchive(Guid id)
    {
        return await _mediaStorageInActiveRepository.GetDetailArchive(id);
    }

    public async Task<IEnumerable<VwArchiveRent>> GetDetails(Guid id)
    {
        var detail = await _mediaStorageInActiveRepository.GetDetailByArchiveId(id);
        var result = await _mediaStorageInActiveRepository.GetDetailByArchiveIdAndSort(detail.MediaStorageInActiveId, detail.Sort);

        return result;
    }
    public async Task<TrxMediaStorageInActive> GetById(Guid id)
    {
        return await _mediaStorageInActiveRepository.GetById(id);
    }
    public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
    {
        try
        {
            var filterData = new DataTableModel();

            filterData.sortColumn = model.columns[model.order[0].column].data;
            filterData.sortColumnDirection = model.order[0].dir;
            filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
            filterData.pageSize = model.length;
            filterData.skip = model.start;

            var dataCount = await _mediaStorageInActiveRepository.GetCountByFilterModel(filterData);
            var results = await _mediaStorageInActiveRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<object>();

            responseModel.draw = model.draw;
            responseModel.recordsTotal = dataCount;
            responseModel.recordsFiltered = dataCount;
            responseModel.data = results.ToList();

            return responseModel;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<int> Delete(Guid ID)
    {
        return await _mediaStorageInActiveRepository.Delete(ID);
    }
    public async Task<int> Insert(TrxMediaStorageInActive model, string[] listSts, string[] listArchive)
    {
        await Task.Delay(0);
        List<TrxMediaStorageInActiveDetail> detail = new();
        string stsId;
        string sort;
        string archiveId;

        foreach (var item in listSts)
        {
            string[] words = item!.Split('#');
            stsId = words[0];
            sort = words[1];

            foreach (var archive in listArchive)
            {
                string[] wordsTwo = archive!.Split('#');
                archiveId = wordsTwo[0];
                var sortArchive = wordsTwo[1];

                if (sort == sortArchive)
                {
                    TrxMediaStorageInActiveDetail detailTwo = new()
                    {
                        SubTypeStorageId = new Guid(stsId),
                        ArchiveId = new Guid(archiveId),
                        Sort = Convert.ToInt16(sort),
                        CreatedBy = model.CreatedBy,
                        CreatedDate = model.CreatedDate
                    };
                    detail.Add(detailTwo);
                }
            }
        }

        return await _mediaStorageInActiveRepository.Insert(model, detail);
    }
    public async Task<int> Update(TrxMediaStorageInActive model, string[] listSts, string[] listArchive)
    {
        await Task.Delay(0);
        List<TrxMediaStorageInActiveDetail> detail = new();
        string stsId;
        string sort;
        string archiveId;

        foreach (var item in listSts)
        {
            string[] words = item!.Split('#');
            stsId = words[0];
            sort = words[1];

            foreach (var archive in listArchive)
            {
                string[] wordsTwo = archive!.Split('#');
                archiveId = wordsTwo[0];
                var sortArchive = wordsTwo[1];

                if (sort == sortArchive)
                {
                    TrxMediaStorageInActiveDetail detailTwo = new()
                    {
                        SubTypeStorageId = new Guid(stsId),
                        ArchiveId = new Guid(archiveId),
                        Sort = Convert.ToInt16(sort),
                        CreatedBy = model.CreatedBy,
                        CreatedDate = model.CreatedDate
                    };
                    detail.Add(detailTwo);
                }
            }
        }

        return await _mediaStorageInActiveRepository.Update(model, detail);
    }
}
