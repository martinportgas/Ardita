using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IGmdRepository
{
    Task<IEnumerable<MstGmd>> GetById(Guid id);
    Task<IEnumerable<MstGmd>> GetAll();
    Task<IEnumerable<MstGmd>> GetByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> Insert(MstGmd model);
    Task<int> Delete(MstGmd model);
    Task<int> Update(MstGmd model);
}
