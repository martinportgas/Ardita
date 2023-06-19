using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveOutIndicatorService
{
    Task<TrxArchiveOutIndicator> GetByUseAndDate(Guid archiveId, string usedBy, DateTime usedDate);
    Task<IEnumerable<TrxArchiveOutIndicator>> GetByMediaStorageId(Guid mediaStorageId);
    Task<int> Insert(TrxArchiveOutIndicator model);
    Task<int> Update(TrxArchiveOutIndicator model);
    Task<bool> InsertBulk(List<TrxArchiveOutIndicator> models);
    Task<bool> Process(Guid mediaStorageId, string detailIsUsed, string usedBy, string usedDate, string returnDate, Guid userId);
}
