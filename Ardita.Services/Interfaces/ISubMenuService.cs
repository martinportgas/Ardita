using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface ISubMenuService
    {
        Task<MstSubmenu> GetById(Guid id);
        Task<IEnumerable<MstSubmenu>> GetAll(string par = " 1=1 ");
        Task<int> Insert(MstSubmenu model);
        Task<int> Delete(MstSubmenu model);
        Task<int> Update(MstSubmenu model);
        Task<List<SubMenuTypes>> GetSubMenuTypeToLookUp();
        Task<List<SubMenuTypes>> GetSubMenuTypeToLookUp(Guid id);
    }
}
