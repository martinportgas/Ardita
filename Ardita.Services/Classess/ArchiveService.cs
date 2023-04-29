using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class ArchiveService : IArchiveService
{
    private readonly IArchiveUnitRepository _archiveUnitRepository;
    private readonly IArchiveRepository _archiveRepository;

    public ArchiveService(
        IArchiveUnitRepository archiveUnitRepository,
        IArchiveRepository archiveRepository
        )
    {
        _archiveUnitRepository = archiveUnitRepository;
        _archiveRepository = archiveRepository;
    }

    public Task<int> Delete(TrxArchive model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxArchive>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<TrxArchive> GetById(Guid id)
    {
        return await _archiveRepository.GetById(id);
    }

    public async Task<DataTableResponseModel<TrxArchive>> GetList(DataTablePostModel model)
    {
        try
        {
            var dataCount = await _archiveRepository.GetCount();

            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].data,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start
            };

            var results = await _archiveRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<TrxArchive>
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

    public async Task<DataTableResponseModel<TrxArchive>> GetListForMonitoring(DataTablePostModel model)
    {
        try
        {
            var dataCount = await _archiveRepository.GetCountForMonitoring(model.PositionId);

            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].data,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start,
                PositionId = model.PositionId
            };

            var results = await _archiveRepository.GetByFilterModelForMonitoring(filterData);

            var responseModel = new DataTableResponseModel<TrxArchive>
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

    public Task<int> Insert(TrxArchive model)
    {
        throw new NotImplementedException();
    }

    public Task<int> Update(TrxArchive model)
    {
        throw new NotImplementedException();
    }
}
