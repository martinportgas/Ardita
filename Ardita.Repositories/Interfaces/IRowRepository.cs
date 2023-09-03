using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IRowRepository
{
    Task<TrxRow> GetById(Guid id);
    Task<IEnumerable<TrxRow>> GetAll(string par = " 1=1 ");
    Task<IEnumerable<TrxRow>> GetRowNotAvailable();
    Task<IEnumerable<TrxRow>> GetRowAvailable(IEnumerable<TrxRow> listRowNotAvailable);
    Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
    Task<int> GetCount(DataTableModel model);
    Task<int> Insert(TrxRow model);
    Task<bool> InsertBulk(List<TrxRow> rows);
    Task<int> Delete(TrxRow model);
    Task<int> Update(TrxRow model);
}
