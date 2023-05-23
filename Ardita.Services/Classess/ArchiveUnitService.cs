using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class ArchiveUnitService : IArchiveUnitService
{
    private readonly IArchiveUnitRepository _archiveUnitRepository;

    public ArchiveUnitService(IArchiveUnitRepository archiveUnitRepository) => _archiveUnitRepository = archiveUnitRepository;

    public async Task<int> Delete(TrxArchiveUnit model) => await _archiveUnitRepository.Delete(model);

    public async Task<IEnumerable<TrxArchiveUnit>> GetAll() => await _archiveUnitRepository.GetAll();

    public async Task<TrxArchiveUnit> GetById(Guid id) => await _archiveUnitRepository.GetById(id);

    public async Task<IEnumerable<TrxArchiveUnit>> GetByListArchiveUnit(List<string> listArchiveUnitCode)
    {
        return await _archiveUnitRepository.GetByListArchiveUnit(listArchiveUnitCode);
    }

    public async Task<DataTableResponseModel<TrxArchiveUnit>> GetList(DataTablePostModel model)
    {
        try
        {
            var dataCount = await _archiveUnitRepository.GetCount();

            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].data,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start
            };

            var results = await _archiveUnitRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<TrxArchiveUnit>
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

    public async Task<int> Insert(TrxArchiveUnit model) => await _archiveUnitRepository.Insert(model);

    public async Task<bool> InsertBulk(List<TrxArchiveUnit> trxArchiveUnits)
    {
        return await _archiveUnitRepository.InsertBulk(trxArchiveUnits);
    }

    public async Task<int> Update(TrxArchiveUnit model) => await _archiveUnitRepository.Update(model);
}
