using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<IdxUserRole> GetById(Guid id);
        Task<IdxUserRole> GetByUserAndRoleId(Guid id, Guid role);
        Task<IEnumerable<IdxUserRole>> GetIdxUserRoleByUserId(Guid id);
        Task<IEnumerable<IdxUserRole>> GetAll();
        Task<int> Insert(IdxUserRole model);
        Task<int> Delete(IdxUserRole model);
        Task<int> Update(IdxUserRole model);
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCountByFilterModel(DataTableModel model);
    }
}
