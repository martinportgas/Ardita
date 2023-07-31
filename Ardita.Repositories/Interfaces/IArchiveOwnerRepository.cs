using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveOwnerRepository
{
    Task<IEnumerable<MstArchiveOwner>> GetById(Guid id);
    Task<IEnumerable<MstArchiveOwner>> GetAll();
    Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
    Task<int> GetCountByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> Insert(MstArchiveOwner model);
    Task<bool> InsertBulk(List<MstArchiveOwner> MstArchiveOwners);
    Task<int> Delete(MstArchiveOwner model);
    Task<int> Update(MstArchiveOwner model);
}
