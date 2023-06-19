using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IMediaStorageService
{
    Task<TrxMediaStorage> GetById(Guid id);
    Task<TrxMediaStorageDetail> GetDetailByArchiveId(Guid id);
    Task<IEnumerable<TrxMediaStorage>> GetAll();
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxMediaStorage model, string[] archiveId);
    Task<int> Delete(TrxMediaStorage model);
    Task<int> Update(TrxMediaStorage model, string[] archiveId);
    Task<int> UpdateDetail(TrxMediaStorageDetail model);
    Task<bool> UpdateDetailIsUsed(Guid archiveId, string usedBy, bool isUsed);
}
