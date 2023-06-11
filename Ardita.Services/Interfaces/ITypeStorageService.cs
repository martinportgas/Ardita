using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface ITypeStorageService
{
    Task<TrxTypeStorage> GetById(Guid id);
    Task<IEnumerable<TrxTypeStorage>> GetAll();
    Task<IEnumerable<TrxTypeStorageDetail>> GetAllByTypeStorageId(Guid TypeStorageId);
    Task<DataTableResponseModel<TrxTypeStorage>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxTypeStorage model);
    Task<bool> InsertBulk(List<TrxTypeStorage> model);
    Task<int> Delete(TrxTypeStorage model);
    Task<int> Update(TrxTypeStorage model);
}
