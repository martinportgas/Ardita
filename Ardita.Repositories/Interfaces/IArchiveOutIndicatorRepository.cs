using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveOutIndicatorRepository
{
    Task<int> Insert(TrxArchiveOutIndicator model);
    Task<bool> InsertBulk(List<TrxArchiveOutIndicator> models);
    Task<int> Update(TrxArchiveOutIndicator model);
    Task<TrxArchiveOutIndicator> GetByUseAndDate(Guid archiveId, string usedBy, DateTime usedDate);
    Task<IEnumerable<TrxArchiveOutIndicator>> GetByMediaStorageId(Guid mediaStorageId);
}
