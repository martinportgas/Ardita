using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Menus;
using Ardita.Models.ViewModels.Pages;
using Ardita.Models.ViewModels.Positions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IMenuService
    {
        Task<MstMenu> GetById(Guid id);
        Task<IEnumerable<MstMenu>> GetAll();
        Task<DataTableResponseModel<MstMenu>> GetListMenu(DataTablePostModel model);
        Task<int> Insert(MstMenu model);
        Task<int> Delete(MstMenu model);
        Task<int> Update(MstMenu model);
        Task<List<MenuTypes>> GetMenuToLookUp();
    }
}
