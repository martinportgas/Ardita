using Ardita.Models.DbModels;

namespace Ardita.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<IdxUserRole>> GetById(Guid id);
        Task<IEnumerable<IdxUserRole>> GetAll();
        Task<int> Insert(IdxUserRole model);
        Task<int> Delete(IdxUserRole model);
        Task<int> Update(IdxUserRole model);
    }
}
