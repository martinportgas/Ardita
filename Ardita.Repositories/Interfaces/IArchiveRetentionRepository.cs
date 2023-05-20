using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveRetentionRepository
{
    Task<IEnumerable<VwArchiveRetention>> GetAll();
    Task<IEnumerable<VwArchiveRetentionInActive>> GetInActiveAll();
    Task<int> GetCount();
    Task<IEnumerable<object>> GetArchiveRetentionByFilterModel(DataTableModel model);
    Task<int> GetCountArchiveRetentionByFilterModel(DataTableModel model);
}
