using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.RolePages;

namespace Ardita.Services.Interfaces
{
    public interface IRolePageService
    {
        Task<IEnumerable<IdxRolePage>> GetById(Guid id);
        Task<IEnumerable<IdxRolePage>> GetAll();
        Task<RolePageListViewModel> GetListRolePages(Guid id);
        Task<IEnumerable<RolePageTreeViewModel>> GetTreeRolePages(Guid id);
        Task<int> Insert(IdxRolePage model);
        Task<bool> InsertBulk(List<IdxRolePage> model);
        Task<int> Delete(IdxRolePage model);
        Task<int> DeleteByRoleId(Guid roleId);
        Task<int> Update(IdxRolePage model);
    }
}
