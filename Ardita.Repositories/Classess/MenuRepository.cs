using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class MenuRepository : IMenuRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public MenuRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }
        public Task<int> Delete(MstMenu model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MstMenu>> GetAll()
        {
            var result = await _context.MstMenus
                .Include(x => x.MstSubmenus)
                .AsNoTracking()
                .Where(x => x.IsActive == true)
                .ToListAsync();
            return result;
        }

        public async  Task<IEnumerable<MstMenu>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<MstMenu> result;

            var propertyInfo = typeof(MstMenu).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(MstMenu).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.MstMenus
                .Include(x => x.MstSubmenus.OrderBy(z => z.Sort))
                .Where(
                    x => (x.Name).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderBy(x => EF.Property<MstMenu>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.MstMenus
                .Include(x => x.MstSubmenus.OrderBy(z => z.Sort))
                .Where(
                    x => (x.Name).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderByDescending(x => EF.Property<MstMenu>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<MstMenu> GetById(Guid id)
        {
            var result = await _context.MstMenus
                .Include(x => x.MstSubmenus)
                .AsNoTracking().FirstAsync(x => x.MenuId == id);
            return result;
        }

        public async Task<int> GetCount()
        {
            var results = await _context.MstMenus.AsNoTracking().Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<int> Insert(MstMenu model)
        {
            int result = 0;
            var menus = await _context.MstMenus.FirstAsync(x => x.MenuId == model.MenuId);
            if (menus != null)
            {
                _context.MstMenus.Add(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstMenu>(GlobalConst.New, model.CreatedBy, new List<MstMenu> { }, new List<MstMenu> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<int> Update(MstMenu model)
        {
            int result = 0;
            var data = await _context.MstMenus.AsNoTracking().FirstAsync(x => x.IsActive == true && x.MenuId == model.MenuId);

            if (data != null)
            {
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                model.IsActive = true;
                model.Icon = data.Icon;

                _context.Update(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstMenu>(GlobalConst.Update, (Guid)model.UpdateBy!, new List<MstMenu> { data }, new List<MstMenu> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
    }
}
