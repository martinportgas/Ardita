using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveRetentionRepository
{
    Task<IEnumerable<VwArchiveRetention>> GetAll(string par = " 1=1 ");
    Task<IEnumerable<VwArchiveRetentionInActive>> GetInActiveAll(string par = " 1=1 ");
    Task<int> GetCount();
    Task<IEnumerable<object>> GetArchiveRetentionByFilterModel(DataTableModel model);
    Task<int> GetCountArchiveRetentionByFilterModel(DataTableModel model);
}
