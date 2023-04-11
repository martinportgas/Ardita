using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class SubMenuRepository : ISubMenuRepository
    {
        private readonly BksArditaDevContext _context;
        public SubMenuRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public Task<int> Delete(MstSubmenu model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MstSubmenu>> GetAll()
        {
            var result = await _context.MstSubmenus.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MstSubmenu>> GetById(Guid id)
        {
            var result = await _context.MstSubmenus.AsNoTracking().Where(x => x.MenuId == id).ToListAsync();
            return result;
        }

        public async Task<int> Insert(MstSubmenu model)
        {
            int result = 0;
            var menus = await _context.MstSubmenus.AsNoTracking().Where(x => x.SubmenuId == model.SubmenuId).ToListAsync();
            if (menus.Count == 0)
            {
                _context.MstSubmenus.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(MstSubmenu model)
        {
            int result = 0;
            var menus = await _context.MstSubmenus.AsNoTracking().Where(x => x.SubmenuId == model.SubmenuId).ToListAsync();
            if (menus.Count > 0)
            {
                model.MenuId = menus.FirstOrDefault().MenuId;
                model.Path = menus.FirstOrDefault().Path;
                model.CreatedBy = menus.FirstOrDefault().CreatedBy;
                model.CreatedDate = menus.FirstOrDefault().CreatedDate;

                _context.MstSubmenus.Update(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
    }
}
