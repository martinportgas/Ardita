using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Archive;
using Microsoft.Extensions.Primitives;

namespace Ardita.Services.Interfaces;

public interface IArchiveService
{
    Task<TrxArchive> GetById(Guid id);
    Task<TrxArchive> GetByCode(string code);
    Task<int> GetCountByLikeCode(string code);
    Task<IEnumerable<TrxArchive>> GetAll(string param = " 1=1 ");
    Task<int> GetCount(string par = " 1=1 ");
    Task<IEnumerable<TrxArchive>> GetByParams(string param = "1=1");
    Task<IEnumerable<TrxArchive>> GetAllInActive(string param = " 1=1 ");
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<IEnumerable<ArchiveExportModel>> GetExportList(DataTablePostModel model);
    Task<int> Insert(TrxArchive model, StringValues modelDetail);
    Task<bool> InsertBulk(List<TrxArchive> trxArchives);
    Task<int> Delete(TrxArchive model);
    Task<int> Update(TrxArchive model, StringValues files, string[] filesDeleted);
    Task<int> Submit(TrxArchive model);
    Task<IEnumerable<object>> GetAvailableArchiveBySubSubjectId(Guid subSubjectId, Guid mediaStorageId = new Guid(), string year = "", Guid gmdDetailId = new Guid());
    Task<IEnumerable<TrxArchive>> GetArchiveActiveBySubjectId(Guid subSubjectId);
    Task<IEnumerable<TrxArchive>> GetAvailableArchiveInActiveBySubSubjectId(Guid subSubjectId, Guid mediaStorageId = new Guid(), string year = "");
}
