using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface ITypeStorageService
{
    Task<TrxTypeStorage> GetById(Guid id);
    Task<IEnumerable<TrxTypeStorage>> GetAll();
    Task<IEnumerable<TrxTypeStorageDetail>> GetAllByTypeStorageId(Guid TypeStorageId);
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxTypeStorage model, string[] detail);
    Task<bool> InsertBulk(List<TrxTypeStorage> model);
    Task<int> Delete(TrxTypeStorage model);
    Task<int> Update(TrxTypeStorage model, string[] detail);
}
