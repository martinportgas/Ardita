using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.Roles;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<MstRole>> GetById(Guid id);
        Task<IEnumerable<MstRole>> GetAll();
        Task<RoleListViewModel> GetListRole(DataTableModel tableModel);
        Task<int> Insert(MstRole model);
        Task<int> Delete(MstRole model);
        Task<int> Update(MstRole model);
    }
}
