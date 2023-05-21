using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using Castle.Components.DictionaryAdapter.Xml;

namespace Ardita.Services.Classess;

public class SubTypeStorageService : ISubTypeStorageService
{
    private readonly ISubTypeStorageRepository _subTypeStorageRepository;
        
    public SubTypeStorageService(ISubTypeStorageRepository subTypeStorageRepository) => _subTypeStorageRepository = subTypeStorageRepository;
    public async Task<int> Delete(MstSubTypeStorage model)
    {
        return await _subTypeStorageRepository.Delete(model);
    }
    public async Task<int> DeleteIDXSubTypeStorage(Guid id)
    {
        return await _subTypeStorageRepository.DeleteIDXSubTypeStorage(id);
    }
    public async Task<IEnumerable<MstSubTypeStorage>> GetAll()
    {
        return await _subTypeStorageRepository.GetAll();
    }
    public async Task<IEnumerable<MstSubTypeStorage>> GetAllByTypeStorageId(Guid ID) => await _subTypeStorageRepository.GetAllByTypeStorageId(ID);

    public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
    {
        try
        {
            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].data,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start
            };

            var dataCount = await _subTypeStorageRepository.GetCountByFilterModel(filterData);
            var results = await _subTypeStorageRepository.GetByFilterModel(filterData);

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

    public async Task<IEnumerable<MstSubTypeStorage>> GetById(Guid id)
    {
        return await _subTypeStorageRepository.GetById(id);
    }
    public async Task<int> Insert(MstSubTypeStorage model)
    {
        return await _subTypeStorageRepository.Insert(model);
    }
    public async Task<bool> InsertBulk(List<MstSubTypeStorage> mstSubTypeStorages)
    {
        return await _subTypeStorageRepository.InsertBulk(mstSubTypeStorages);
    }
    public async Task<bool> InsertBulkIDXTypeStorage(List<IdxSubTypeStorage> idxSubTypeStorages)
    {
        return await _subTypeStorageRepository.InsertBulkIDXTypeStorage(idxSubTypeStorages);
    }
    public async Task<int> InsertIDXSubTypeStorage(IdxSubTypeStorage model)
    {
        return await _subTypeStorageRepository.InsertIDXSubTypeStorage(model);
    }
    public async Task<int> Update(MstSubTypeStorage model)
    {
        return await _subTypeStorageRepository.Update(model);
    }

    public async Task<IEnumerable<IdxSubTypeStorage>> GetAllBySubTypeStorageId(Guid ID)
    {
        return await _subTypeStorageRepository.GetAllBySubTypeStorageId(ID);
    }
}
