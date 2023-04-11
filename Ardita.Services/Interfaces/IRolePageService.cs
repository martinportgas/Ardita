using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.RolePages;
using Ardita.Models.ViewModels.UserRoles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IRolePageService
    {
        Task<IEnumerable<MstRolePage>> GetById(Guid id);
        Task<IEnumerable<MstRolePage>> GetAll();
        Task<RolePageListViewModel> GetListRolePages(Guid id);
        Task<IEnumerable<RolePageTreeViewModel>> GetTreeRolePages(Guid id);
        Task<int> Insert(MstRolePage model);
        Task<bool> InsertBulk(List<MstRolePage> model);
        Task<int> Delete(MstRolePage model);
        Task<int> DeleteByRoleId(Guid roleId);
        Task<int> Update(MstRolePage model);
    }
}
