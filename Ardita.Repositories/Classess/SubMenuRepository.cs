using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class SubMenuRepository : ISubMenuRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public SubMenuRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }

        public Task<int> Delete(MstSubmenu model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MstSubmenu>> GetAll(string par = " 1=1 ")
        {
            var result = await _context.MstSubmenus.Where(x => x.IsActive == true)
                .Where(par).AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<MstSubmenu> GetById(Guid id)
        {
            var result = await _context.MstSubmenus.AsNoTracking().FirstAsync(x => x.SubmenuId == id && x.IsActive == true);
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

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstSubmenu>(GlobalConst.New, (Guid)model.CreatedBy!, new List<MstSubmenu> {  }, new List<MstSubmenu> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<int> Update(MstSubmenu model)
        {
            int result = 0;
            var menus = await _context.MstSubmenus.AsNoTracking().FirstAsync(x => x.SubmenuId == model.SubmenuId && x.IsActive == true);
            if (menus != null)
            {
                model.MenuId = menus.MenuId;
                model.Path = menus.Path;
                model.CreatedBy = menus.CreatedBy;
                model.CreatedDate = menus.CreatedDate;
                model.IsActive = true;

                _context.MstSubmenus.Update(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstSubmenu>(GlobalConst.Update, (Guid)model.CreatedBy!, new List<MstSubmenu> { menus }, new List<MstSubmenu> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
    }
}
