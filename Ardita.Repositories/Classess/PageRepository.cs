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
                var data = await _context.MstPages.AsNoTracking().Where(x => x.PageId == model.PageId).ToListAsync();
                if (data != null)
                {
                    model.IsActive = false;
                    model.CreatedBy = data.FirstOrDefault().CreatedBy;
                    model.CreatedDate = data.FirstOrDefault().CreatedDate;

                    _context.MstPages.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
        public async Task<IEnumerable<MstPage>> GetAll()
        {
            var result = await _context.MstPages.Where(x => x.IsActive == true).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<MstPage>> GetById(Guid id)
        {
            var result = await _context.MstPages.AsNoTracking().Where(x=> x.PageId == id && x.IsActive == true).ToListAsync();
            return result;
        }
        public async Task<int> Insert(MstPage model)
        {
            int result = 0;
            if (model != null)
            {
                var data = await _context.MstPages.AsNoTracking().Where(
                    x => x.SubmenuId == model.SubmenuId &&
                    x.Name == model.Name &&
                    x.Path == model.Path
                    ).ToListAsync();

                model.IsActive = true;

                if (data.Count > 0)
                {
                    model.PageId = data.FirstOrDefault().PageId;
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
                var data = await _context.MstPages.AsNoTracking().Where(x => x.PageId == model.PageId).ToListAsync();
                if (data != null)
                {
                    model.IsActive = true;
                    model.CreatedBy = data.FirstOrDefault().CreatedBy;
                    model.CreatedDate = data.FirstOrDefault().CreatedDate;
                    _context.MstPages.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
