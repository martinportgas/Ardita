using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IPageRepository
    {
        Task<MstPage> GetById(Guid id);
        Task<IEnumerable<MstPage>> GetAll(string par = " 1=1 ");
        Task<IEnumerable<MstPage>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> GetCountBySubMenuId(Guid? SubMenuId);
        Task<int> Insert(MstPage model);
        Task<int> Delete(MstPage model);
        Task<int> Update(MstPage model);
    }
}
