using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveCreatorRepository
{
    Task<IEnumerable<MstCreator>> GetById(Guid id);
    Task<IEnumerable<MstCreator>> GetAll();
    Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
    Task<int> GetCountByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> Insert(MstCreator model);
    Task<bool> InsertBulk(List<MstCreator> mstCreators);
    Task<int> Delete(MstCreator model);
    Task<int> Update(MstCreator model);
}
