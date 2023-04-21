using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveCreatorRepository
{
    Task<IEnumerable<MstCreator>> GetById(Guid id);
    Task<IEnumerable<MstCreator>> GetAll();
    Task<IEnumerable<MstCreator>> GetByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> Insert(MstCreator model);
    Task<int> Delete(MstCreator model);
    Task<int> Update(MstCreator model);
}
