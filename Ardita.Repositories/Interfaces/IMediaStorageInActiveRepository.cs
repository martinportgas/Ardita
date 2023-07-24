using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IMediaStorageInActiveRepository
{
    Task<IEnumerable<TrxMediaStorageInActive>> GetAll();
    Task<TrxMediaStorageInActive> GetById(Guid id);
    Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
    Task<int> GetCountByFilterModel(DataTableModel model);
    Task<TrxMediaStorageInActiveDetail> GetDetailByArchiveId(Guid id);
    Task<IEnumerable<VwArchiveRent>> GetDetailByArchiveIdAndSort(Guid id, int sort);
    Task<IEnumerable<object>> GetDetailArchive(Guid id);
    Task<int> Insert(TrxMediaStorageInActive model, List<TrxMediaStorageInActiveDetail> detail);
    Task<int> Update(TrxMediaStorageInActive model, List<TrxMediaStorageInActiveDetail> detail);
    Task<int> Delete(Guid ID);
    Task<IEnumerable<VwArchiveRent>> GetArchiveRent();

}
