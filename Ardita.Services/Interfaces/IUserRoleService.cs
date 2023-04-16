using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.UserRoles;

namespace Ardita.Services.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<IdxUserRole>> GetById(Guid id);
        Task<IEnumerable<IdxUserRole>> GetAll();
        Task<UserRoleListViewModel> GetListUserRoles(Guid Id);
        Task<int> Insert(IdxUserRole model);
        Task<int> Delete(IdxUserRole model);
        Task<int> Update(IdxUserRole model);
    }
}
