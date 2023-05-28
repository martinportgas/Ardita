using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

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

    public async Task<IEnumerable<object>> GetDetails(Guid id)
    {
        var detail = await _mediaStorageInActiveRepository.GetDetailByArchiveId(id);
        var result = await _mediaStorageInActiveRepository.GetDetailByArchiveIdAndSort(detail.ArchiveId, detail.Sort);

        return result;
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
}
