using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.Positions;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardita.Models.ViewModels.Roles;
using Ardita.Models.ViewModels.Users;
using Ardita.Models.ViewModels.UserRoles;

namespace Ardita.Services.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<MstUserRole>> GetById(Guid id);
        Task<IEnumerable<MstUserRole>> GetAll();
        Task<UserRoleListViewModel> GetListUserRoles(Guid Id);
        Task<int> Insert(MstUserRole model);
        Task<int> Delete(MstUserRole model);
        Task<int> Update(MstUserRole model);
    }
}
