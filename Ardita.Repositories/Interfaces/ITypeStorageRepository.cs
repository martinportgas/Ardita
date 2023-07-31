using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface ITypeStorageRepository
{
    Task<TrxTypeStorage> GetById(Guid id);
    Task<IEnumerable<TrxTypeStorage>> GetAll();
    Task<IEnumerable<TrxTypeStorageDetail>> GetAllByTypeStorageId(Guid TypeStorageId);
    Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
    Task<int> GetCountByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> Insert(TrxTypeStorage model, List<TrxTypeStorageDetail> detail);
    Task<bool> InsertBulk(List<TrxTypeStorage> rows);
    Task<int> Delete(TrxTypeStorage model);
    Task<int> Update(TrxTypeStorage model, List<TrxTypeStorageDetail> detail);
}
