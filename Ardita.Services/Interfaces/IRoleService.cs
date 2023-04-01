using Ardita.Models.DbModels;
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
        Task<int> Insert(MstRole model);
        Task<int> Delete(MstRole model);
        Task<int> Update(MstRole model);
    }
}
