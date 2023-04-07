using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<MstUserRole>> GetById(Guid id);
        Task<IEnumerable<MstUserRole>> GetAll();
        Task<int> Insert(MstUserRole model);
        Task<int> Delete(MstUserRole model);
        Task<int> Update(MstUserRole model);
    }
}
