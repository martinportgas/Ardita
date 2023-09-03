using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveTypeRepository
{
    Task<IEnumerable<MstArchiveType>> GetById(Guid id);
    Task<IEnumerable<MstArchiveType>> GetAll(string par = " 1=1 ");
    Task<IEnumerable<MstArchiveType>> GetByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> Insert(MstArchiveType model);
    Task<bool> InsertBulk(List<MstArchiveType> MstArchiveTypes);
    Task<int> Delete(MstArchiveType model);
    Task<int> Update(MstArchiveType model);
}
