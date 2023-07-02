using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveRepository
{
    Task<TrxArchive> GetById(Guid id);
    Task<IEnumerable<TrxArchive>> GetAll(List<string> listArchiveUnitCode = null);
    Task<IEnumerable<TrxArchive>> GetAllInActive(List<string> listArchiveUnitCode);
    Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> GetCountByFilterData(DataTableModel model);
    Task<int> Insert(TrxArchive model, List<FileModel> files, string path = ""); 
    Task<int> Submit(Guid ArchiveId); 
    Task<bool> InsertBulk(List<TrxArchive> trxArchives);
    Task<int> Delete(TrxArchive model);
    Task<int> Update(TrxArchive model, List<FileModel> files, List<Guid> filesDeletedId, string path = "");
    Task<IEnumerable<TrxArchive>> GetAvailableArchiveBySubSubjectId(Guid subSubjectId, Guid mediaStorageId = new Guid(), string year = "", Guid gmdDetailId = new Guid());
    Task<IEnumerable<TrxArchive>> GetAvailableArchiveInActiveBySubSubjectId(Guid subSubjectId, Guid mediaStorageId = new Guid(), string year = "");
    Task<IEnumerable<TrxArchive>> GetArchiveActiveBySubjectId(Guid subSubjectId);
    Task<string> GetPathArchive(Guid SubSubjectClassificationId, DateTime CreatedDateArchive);
    Task<int> Submit(TrxArchive model);

    Task<IEnumerable<TrxArchive>> GetReportArchiveActive();
}
