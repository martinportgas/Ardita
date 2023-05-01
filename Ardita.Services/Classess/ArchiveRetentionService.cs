using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class ArchiveRetentionService : IArchiveRetentionService
{
    private readonly IArchiveRetentionRepository _archiveRetentionRepository;

    public ArchiveRetentionService(IArchiveRetentionRepository archiveRetentionRepository) => _archiveRetentionRepository = archiveRetentionRepository;

    public Task<int> Delete(TrxArchive model)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<VwArchiveRetention>> GetAll()
    {
        var results = await _archiveRetentionRepository.GetAll();
        return results;
    }

    public Task<IEnumerable<TrxArchive>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<DataTableResponseModel<VwArchiveRetention>> GetList(DataTablePostModel model)
    {
        try
        {
            var dataCount = await _archiveRetentionRepository.GetCount();

            var filterData = new DataTableModel();

            filterData.sortColumn = model.columns[model.order[0].column].data;
            filterData.sortColumnDirection = model.order[0].dir;
            filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
            filterData.pageSize = model.length;
            filterData.skip = model.start;

            var results = await _archiveRetentionRepository.GetArchiveRetentionByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<VwArchiveRetention>();

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

    public Task<int> Insert(TrxArchive model)
    {
        throw new NotImplementedException();
    }

    public Task<int> Update(TrxArchive model)
    {
        throw new NotImplementedException();
    }
}
