using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class TypeStorageService : ITypeStorageService
{
    private readonly ITypeStorageRepository _TypeStorageRepository;

    public TypeStorageService(ITypeStorageRepository TypeStorageRepository) => _TypeStorageRepository = TypeStorageRepository;

    public async Task<int> Delete(TrxTypeStorage model) => await _TypeStorageRepository.Delete(model);

    public async Task<IEnumerable<TrxTypeStorage>> GetAll() => await _TypeStorageRepository.GetAll();

    public async Task<TrxTypeStorage> GetById(Guid id)
    => await _TypeStorageRepository.GetById(id);

    public async Task<DataTableResponseModel<TrxTypeStorage>> GetList(DataTablePostModel model)
    {
        try
        {
            var dataCount = await _TypeStorageRepository.GetCount();

            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].data,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start
            };

            var results = await _TypeStorageRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<TrxTypeStorage>
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

    public async Task<int> Insert(TrxTypeStorage model) => await _TypeStorageRepository.Insert(model);
    public async Task<bool> InsertBulk(List<TrxTypeStorage> model) => await _TypeStorageRepository.InsertBulk(model);

    public async Task<int> Update(TrxTypeStorage model) => await _TypeStorageRepository.Update(model);
}
