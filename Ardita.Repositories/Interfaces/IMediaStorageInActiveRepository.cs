using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IMediaStorageInActiveRepository
{
    Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
    Task<int> GetCountByFilterModel(DataTableModel model);
}
