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
    public class PageRepository : IPageRepository
    {
        private readonly BksArditaDevContext _context;
        public PageRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(MstPage model)
        {
            int result = 0;

            if (model.PageId != Guid.Empty)
            {
                var data = await _context.MstPages
                    .Include(x => x.Submenu)
                    .Include(x => x.MstPageDetails)
                    .Include(x => x.IdxRolePages).ThenInclude(x => x.Role)
              
                    .AsNoTracking().FirstAsync(x => x.PageId == model.PageId);
                if (data != null)
                {
                    model.IsActive = false;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;

                    _context.MstPages.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
        public async Task<IEnumerable<MstPage>> GetAll()
        {
            var result = await _context.MstPages
                   .Include(x => x.Submenu)
                    .Include(x => x.MstPageDetails)
                    .Include(x => x.IdxRolePages).ThenInclude(x => x.Role)
                    .AsNoTracking()
                .Where(x => x.IsActive == true).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<MstPage>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<MstPage> result;

            var propertyInfo = typeof(MstPage).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(MstPage).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.MstPages
                  .Include(x => x.Submenu)
                .Where(
                    x => (x.Name).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderBy(x => EF.Property<MstPage>(x, propertyName))
                //.Skip(model.skip)
                //.Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.MstPages
                  .Include(x => x.Submenu)
                .Where(
                    x => (x.Name).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderByDescending(x => EF.Property<MstPage>(x, propertyName))
                //.Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            if (model.SubMenuId != null)
            {
                result = result.Where(x => x.SubmenuId.Equals(model.SubMenuId));
            }
            

            return result;
        }
        public async Task<MstPage> GetById(Guid id)
        {
            var result = await _context.MstPages
                   .Include(x => x.Submenu).ThenInclude(x => x.Menu)
                    .Include(x => x.MstPageDetails)
                    .Include(x => x.IdxRolePages).ThenInclude(x => x.Role)
                .AsNoTracking()
                .FirstAsync(x=> x.PageId == id && x.IsActive == true);

            return result;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.MstPages.AsNoTracking().Where(x => x.IsActive == true).CountAsync();
            return results;
        }
        public async Task<int> GetCountBySubMenuId(Guid? SubMenuId)
        {
            var results = await _context.MstPages.AsNoTracking().Where(x => x.IsActive == true && x.SubmenuId == SubMenuId).CountAsync();
            return results;
        }
        public async Task<int> Insert(MstPage model)
        {
            int result = 0;
            if (model != null)
            {
                var data = await _context.MstPages.AsNoTracking().FirstAsync(
                    x => x.SubmenuId == model.SubmenuId &&
                    x.Name == model.Name &&
                    x.Path == model.Path
                    );

                model.IsActive = true;

                if (data != null)
                {
                    model.PageId = data.PageId;
                    model.UpdateBy = model.CreatedBy;
                    model.UpdateDate = DateTime.Now;
                    _context.MstPages.Update(model);
                    result = await _context.SaveChangesAsync();
                }
                else
                {
                    _context.MstPages.Add(model);
                    result = await _context.SaveChangesAsync();
                }
            }

            return result;
        }
        public async Task<int> Update(MstPage model)
        {
            int result = 0;

            if (model.PageId != Guid.Empty)
            {
                var data = await _context.MstPages.AsNoTracking().FirstAsync(x => x.PageId == model.PageId);
                if (data != null)
                {
                    model.IsActive = true;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    _context.MstPages.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
