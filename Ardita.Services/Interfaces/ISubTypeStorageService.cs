using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface ISubTypeStorageService
{
    Task<IEnumerable<MstSubTypeStorage>> GetAllByTypeStorageId(Guid ID);
    Task<IEnumerable<IdxSubTypeStorage>> GetAllBySubTypeStorageId(Guid ID);
    Task<IEnumerable<MstSubTypeStorage>> GetById(Guid id);
    Task<IEnumerable<MstSubTypeStorage>> GetAll();
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Insert(MstSubTypeStorage model);
    Task<int> InsertIDXSubTypeStorage(IdxSubTypeStorage model);
    Task<bool> InsertBulk(List<MstSubTypeStorage> mstSubTypeStorages);
    Task<bool> InsertBulkIDXTypeStorage(List<IdxSubTypeStorage> idxSubTypeStorages);
    Task<int> Delete(MstSubTypeStorage model);
    Task<int> DeleteIDXSubTypeStorage(Guid id);
    Task<int> Update(MstSubTypeStorage model);
}
