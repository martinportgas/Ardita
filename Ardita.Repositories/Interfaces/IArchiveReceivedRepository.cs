using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveReceivedRepository
{
    Task<IEnumerable<object>> GetByFilterModelArchiveMovement(DataTableModel model);
    Task<int> GetCountByFilterDataArchiveMovement(DataTableModel model);
    Task<TrxArchiveMovement> GetById(Guid id);
}
