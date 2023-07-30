using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Report;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System.Web.WebPages;

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
    public async Task<IEnumerable<VwArchiveRetentionInActive>> GetInActiveAll()
    {
        var results = await _archiveRetentionRepository.GetInActiveAll();
        return results;
    }

    public Task<IEnumerable<TrxArchive>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
    {
        try
        {
            DateTime startDate = GlobalConst.MinDate;
            DateTime endDate = GlobalConst.MaxDate;
            bool validStart = DateTime.TryParse(model.columns[0].search.value, out startDate);
            bool validEnd = DateTime.TryParse(model.columns[1].search.value, out endDate);

            var filterData = new DataTableModel();

            filterData.sortColumn = model.columns[model.order[0].column].name;
            filterData.sortColumnDirection = model.order[0].dir;
            filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
            filterData.pageSize = model.length;
            filterData.skip = model.start;
            filterData.IsArchiveActive = model.IsArchiveActive;
            filterData.advanceSearch = new SearchModel
            {
                StartDate = validStart ? startDate : GlobalConst.MinDate,
                EndDate = validEnd ? endDate : GlobalConst.MaxDate,
                Search = model.columns[2].search.value == null ? "1=1" : model.columns[2].search.value
            };
            filterData.SessionUser = model.SessionUser;

            var dataCount = await _archiveRetentionRepository.GetCountArchiveRetentionByFilterModel(filterData);
            var results = await _archiveRetentionRepository.GetArchiveRetentionByFilterModel(filterData);

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

    public Task<int> Insert(TrxArchive model)
    {
        throw new NotImplementedException();
    }

    public Task<int> Update(TrxArchive model)
    {
        throw new NotImplementedException();
    }
}
