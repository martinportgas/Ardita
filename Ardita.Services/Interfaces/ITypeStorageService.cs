using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface ITypeStorageService
{
    Task<IEnumerable<TrxTypeStorage>> GetById(Guid id);
    Task<IEnumerable<TrxTypeStorage>> GetAll();
    Task<DataTableResponseModel<TrxTypeStorage>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxTypeStorage model);
    Task<int> Delete(TrxTypeStorage model);
    Task<int> Update(TrxTypeStorage model);
}
