using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IRolePageRepository
    {
        Task<IEnumerable<MstRolePage>> GetById(Guid id);
        Task<IEnumerable<MstRolePage>> GetAll();
        Task<int> Insert(MstRolePage model);
        Task<int> Delete(MstRolePage model);
        Task<int> Update(MstRolePage model);
    }
}
