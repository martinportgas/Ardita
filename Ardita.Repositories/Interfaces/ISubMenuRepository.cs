using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface ISubMenuRepository
    {
        Task<MstSubmenu> GetById(Guid id);
        Task<IEnumerable<MstSubmenu>> GetAll(string par = " 1=1 ");
        Task<int> Insert(MstSubmenu model);
        Task<int> Delete(MstSubmenu model);
        Task<int> Update(MstSubmenu model);
    }
}
