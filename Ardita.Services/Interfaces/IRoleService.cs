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
        Task<MstRole> GetById(Guid id);
        Task<IEnumerable<MstRole>> GetAll(string par = " 1=1 ");
        Task<DataTableResponseModel<object>> GetListRoles(DataTablePostModel model);
        Task<int> Insert(MstRole model);
        Task<int> Delete(MstRole model);
        Task<int> Update(MstRole model);
    }
}
