using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveRepository
{
    Task<IEnumerable<TrxArchive>> GetById(Guid id);
    Task<IEnumerable<TrxArchive>> GetAll();
    Task<IEnumerable<TrxArchive>> GetByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> Insert(TrxArchive model, List<FileModel> files);
    Task<int> Delete(TrxArchive model);
    Task<int> Update(TrxArchive model);
}
