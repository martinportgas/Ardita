using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveUnitRepository
{
    Task<TrxArchiveUnit> GetById(Guid id);
    Task<IEnumerable<TrxArchiveUnit>> GetAll();
    Task<IEnumerable<TrxArchiveUnit>> GetByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> Insert(TrxArchiveUnit model);
    Task<bool> InsertBulk(List<TrxArchiveUnit> trxArchiveUnits);
    Task<int> Delete(TrxArchiveUnit model);
    Task<int> Update(TrxArchiveUnit model);
}
