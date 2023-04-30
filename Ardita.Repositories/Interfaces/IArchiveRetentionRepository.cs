using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveRetentionRepository
{
    Task<IEnumerable<VwArchiveRetention>> GetAll();
    Task<int> GetCount();
    Task<IEnumerable<VwArchiveRetention>> GetArchiveRetentionByFilterModel(DataTableModel model);
}
