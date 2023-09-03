using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface ISubTypeStorageService
{
    Task<IEnumerable<MstSubTypeStorage>> GetAllByTypeStorageId(Guid ID);
    Task<IEnumerable<MstSubTypeStorage>> GetAllByTypeStorageandGMDDetailId(Guid ID, Guid GMDDetailID);
    Task<IEnumerable<IdxSubTypeStorage>> GetAllBySubTypeStorageId(Guid ID);
    Task<IEnumerable<MstSubTypeStorageDetail>> GetAllDetailBySubTypeStorageId(Guid ID);
    Task<IEnumerable<MstSubTypeStorage>> GetById(Guid id);
    Task<IEnumerable<MstSubTypeStorage>> GetAll(string par = " 1=1 ");
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Insert(MstSubTypeStorage model);
    Task<int> InsertIDXSubTypeStorage(IdxSubTypeStorage model);
    Task<int> InsertGMDSubTypeStorage(MstSubTypeStorageDetail model);
    Task<bool> InsertBulk(List<MstSubTypeStorage> mstSubTypeStorages);
    Task<bool> InsertBulkIDXTypeStorage(List<IdxSubTypeStorage> idxSubTypeStorages);
    Task<bool> InsertBulkGMDTypeStorage(List<MstSubTypeStorageDetail> MstSubTypeStorageDetail);
    Task<int> Delete(MstSubTypeStorage model);
    Task<int> DeleteIDXSubTypeStorage(Guid id);
    Task<int> DeleteGMDSubTypeStorage(Guid id);
    Task<int> Update(MstSubTypeStorage model);
}
