using Ardita.Models.DbModels;
using Ardita.Models.LogModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class TypeStorageService : ITypeStorageService
{
    private readonly ITypeStorageRepository _TypeStorageRepository;

    public TypeStorageService(ITypeStorageRepository TypeStorageRepository) => _TypeStorageRepository = TypeStorageRepository;

    public async Task<int> Delete(TrxTypeStorage model) => await _TypeStorageRepository.Delete(model);

    public async Task<IEnumerable<TrxTypeStorage>> GetAll() => await _TypeStorageRepository.GetAll();
    public async Task<IEnumerable<TrxTypeStorageDetail>> GetAllByTypeStorageId(Guid TypeStorageId) => await _TypeStorageRepository.GetAllByTypeStorageId(TypeStorageId);
    public async Task<TrxTypeStorage> GetById(Guid id)
    => await _TypeStorageRepository.GetById(id);

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
                skip = model.start
            };

            var results = await _TypeStorageRepository.GetByFilterModel(filterData);
            var dataCount = await _TypeStorageRepository.GetCountByFilterModel(filterData);

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

    public async Task<int> Insert(TrxTypeStorage model, string[] detail)
    {

        int result = 0;
        List<TrxTypeStorageDetail> details = await GetDetail(detail);

        result = await _TypeStorageRepository.Insert(model, details);
        return result;
    }
    public async Task<bool> InsertBulk(List<TrxTypeStorage> model) => await _TypeStorageRepository.InsertBulk(model);

    public async Task<int> Update(TrxTypeStorage model, string[] detail)
    {
        List<TrxTypeStorageDetail> details = await GetDetail(detail);

        return await _TypeStorageRepository.Update(model, details);
    }

    private static async Task<List<TrxTypeStorageDetail>> GetDetail(string[] listDetail)
    {
        await Task.Delay(0);
        List<TrxTypeStorageDetail> details = new();


        foreach (var item in listDetail)
        {
            string[] words = item!.Split('#');
            TrxTypeStorageDetail detailTwo = new()
            {
                GmdDetailId = new Guid(words[0]),
                Size = Convert.ToInt32(words[1])
            };

            details.Add(detailTwo);
        }

        return details;
    }
}
