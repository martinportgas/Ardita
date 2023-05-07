using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveRepository
{
    Task<TrxArchive> GetById(Guid id);
    Task<IEnumerable<TrxArchive>> GetAll();
    Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> GetCountByFilterData(DataTableModel model);
    Task<int> Insert(TrxArchive model, List<FileModel> files); 
    Task<bool> InsertBulk(List<TrxArchive> trxArchives);
    Task<int> Delete(TrxArchive model);
    Task<int> Update(TrxArchive model, List<FileModel> files, List<Guid> filesDeletedId);
    Task<IEnumerable<TrxArchive>> GetAvailableArchiveBySubSubjectId(Guid subSubjectId);
}
