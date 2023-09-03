using Ardita.Models.DbModels;

namespace Ardita.Repositories.Interfaces
{
    public interface IRolePageRepository
    {
        Task<IEnumerable<IdxRolePage>> GetById(Guid id);
        Task<IEnumerable<IdxRolePage>> GetAll(string par = " 1=1 ");
        Task<int> Insert(IdxRolePage model);
        Task<bool> InsertBulk(List<IdxRolePage> model);
        Task<int> Delete(IdxRolePage model);
        Task<int> DeleteByRoleId(Guid id);
        Task<int> Update(IdxRolePage model);
    }
}
