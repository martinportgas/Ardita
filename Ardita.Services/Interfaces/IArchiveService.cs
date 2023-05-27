using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Microsoft.Extensions.Primitives;

namespace Ardita.Services.Interfaces;

public interface IArchiveService
{
    Task<TrxArchive> GetById(Guid id);
    Task<IEnumerable<TrxArchive>> GetAll(List<string> listArchiveUnitCode);
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxArchive model, StringValues modelDetail);
    Task<bool> InsertBulk(List<TrxArchive> trxArchives);
    Task<int> Delete(TrxArchive model);
    Task<int> Update(TrxArchive model, StringValues files, string[] filesDeleted);
    Task<IEnumerable<TrxArchive>> GetAvailableArchiveBySubSubjectId(Guid subSubjectId, Guid mediaStorageId = new Guid(), string year = "");
    Task<IEnumerable<TrxArchive>> GetArchiveActiveBySubjectId(Guid subSubjectId);
}
