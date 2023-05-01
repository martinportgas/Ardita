using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class GmdService : IGmdService
{
    private readonly IGmdRepository _GmdRepository;

    public GmdService(IGmdRepository GmdRepository)
    {
        _GmdRepository = GmdRepository;
    }

    public async Task<int> Delete(MstGmd model) => await _GmdRepository.Delete(model);

    public async Task<IEnumerable<MstGmd>> GetAll() => await _GmdRepository.GetAll();

    public async Task<IEnumerable<MstGmd>> GetById(Guid id) => await _GmdRepository.GetById(id);

    public async Task<DataTableResponseModel<MstGmd>> GetList(DataTablePostModel model)
    {
        try
        {
            var dataCount = await _GmdRepository.GetCount();

            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].data,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start
            };

            var results = await _GmdRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<MstGmd>
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

    public async Task<int> Insert(MstGmd model) => await _GmdRepository.Insert(model);

    public async Task<bool> InsertBulk(List<MstGmd> mstGmds)
    {
        return await _GmdRepository.InsertBulk(mstGmds);
    }

    public async Task<int> Update(MstGmd model) => await _GmdRepository.Update(model);
}
