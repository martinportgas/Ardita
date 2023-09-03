using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class RowService : IRowService
{
    private readonly IRowRepository _rowRepository;
    public RowService(IRowRepository rowRepository)
    {
        _rowRepository = rowRepository;
    }
    public async Task<int> Delete(TrxRow model)
    {
        return await _rowRepository.Delete(model);
    }

    public async Task<IEnumerable<TrxRow>> GetAll(string par = " 1=1 ")
    {
        return await _rowRepository.GetAll(par);
    }

    public async Task<TrxRow> GetById(Guid id)
    {
        return await _rowRepository.GetById(id);
    }

    public async Task<int> Insert(TrxRow model)
    {
        return await _rowRepository.Insert(model);
    }

    public async Task<int> Update(TrxRow model)
    {
        return await _rowRepository.Update(model);
    }
    public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
    {
        try
        {
            

            var filterData = new DataTableModel();

            filterData.sortColumn = model.columns[model.order[0].column].name;
            filterData.sortColumnDirection = model.order[0].dir;
            filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
            filterData.pageSize = model.length;
            filterData.skip = model.start;

            var results = await _rowRepository.GetByFilterModel(filterData);
            var dataCount = await _rowRepository.GetCount(filterData);

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

    public async Task<bool> InsertBulk(List<TrxRow> rows)
    {
        return await _rowRepository.InsertBulk(rows);
    }
    
    public async Task<IEnumerable<TrxRow>> GetAvailableRow()
    {
        var listRowNotAvailable = await _rowRepository.GetRowNotAvailable();

        return await _rowRepository.GetRowAvailable(listRowNotAvailable);
    }
}
