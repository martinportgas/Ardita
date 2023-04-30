using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.Pages;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class SubMenuService : ISubMenuService
    {
        private readonly ISubMenuRepository _subMenuRepository;
        public SubMenuService(ISubMenuRepository subMenuRepository)
        {
            _subMenuRepository = subMenuRepository;
        }

        public async Task<int> Delete(MstSubmenu model)
        {
            return await _subMenuRepository.Delete(model);
        }

        public async Task<IEnumerable<MstSubmenu>> GetAll()
        {
            return await _subMenuRepository.GetAll();
        }

        public async Task<MstSubmenu> GetById(Guid id)
        {
            return await _subMenuRepository.GetById(id);
        }

        public async Task<List<SubMenuTypes>> GetSubMenuTypeToLookUp()
        {
            List<SubMenuTypes> ListResult = new List<SubMenuTypes>();
            SubMenuTypes subMenu;
            var model = await _subMenuRepository.GetAll();
            model = model.OrderBy(x => x.Menu.Sort).ThenBy(x => x.Sort);
            foreach (var item in model)
            {
                subMenu = new SubMenuTypes();
                subMenu.Id = item.SubmenuId;
                subMenu.Name = item.Menu.Name + " - " + item.Name;
                ListResult.Add(subMenu);
            }
            return ListResult;
        }

        public async Task<List<SubMenuTypes>> GetSubMenuTypeToLookUp(Guid id)
        {
            List<SubMenuTypes> ListResult = new List<SubMenuTypes>();
            SubMenuTypes subMenu;   
            var model = await _subMenuRepository.GetAll();
            model = model.Where(x=> x.MenuId == id).OrderBy(x => x.Menu.Sort).ThenBy(x => x.Sort);

            foreach (var item in model)
            {
                subMenu = new SubMenuTypes();
                subMenu.Id = item.SubmenuId;
                subMenu.Name = item.Menu.Name + " - " + item.Name;
                ListResult.Add(subMenu);
            }
            return ListResult;
        }

        public async Task<int> Insert(MstSubmenu model)
        {
            return await _subMenuRepository.Insert(model);
        }

        public Task<int> Update(MstSubmenu model)
        {
            return _subMenuRepository.Update(model);
        }
    }
}
