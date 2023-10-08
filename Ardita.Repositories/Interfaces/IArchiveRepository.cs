using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Archive;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveRepository
{
    Task<TrxArchive> GetById(Guid id);
    Task<TrxArchive> GetByCode(string code);
    Task<int> GetCountByLikeCode(string code);
    Task<IEnumerable<TrxArchive>> GetAll(string par = " 1=1 ");
    Task<int> GetCount(string par = " 1=1 ");
    Task<IEnumerable<TrxArchive>> GetByParams(string param);
    Task<IEnumerable<TrxArchive>> GetAllInActive(string par = " 1=1 ");
    Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
    Task<IEnumerable<ArchiveExportModel>> GetExportByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> GetCountByFilterData(DataTableModel model);
    Task<int> Insert(TrxArchive model, List<FileModel> files, string path = ""); 
    Task<int> Submit(Guid ArchiveId); 
    Task<bool> InsertBulk(List<TrxArchive> trxArchives);
    Task<int> Delete(TrxArchive model);
    Task<int> Update(TrxArchive model, List<FileModel> files, List<Guid> filesDeletedId, string path = "");
    Task<IEnumerable<object>> GetAvailableArchiveBySubSubjectId(Guid subSubjectId, Guid mediaStorageId = new Guid(), string year = "", Guid gmdDetailId = new Guid());
    Task<IEnumerable<TrxArchive>> GetAvailableArchiveInActiveBySubSubjectId(Guid subSubjectId, Guid mediaStorageId = new Guid(), string year = "");
    Task<IEnumerable<TrxArchive>> GetArchiveActiveBySubjectId(Guid subSubjectId, Guid formId);
    Task<string> GetPathArchive(Guid SubSubjectClassificationId, DateTime CreatedDateArchive);
    Task<int> Submit(TrxArchive model);

    Task<IEnumerable<TrxArchive>> GetReportArchiveActive();
}
