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
            if (model != null)
            {
                _context.MstPages.Remove(model);
                await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<MstPage>> GetAll()
        {
            var result = await _context.MstPages.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MstPage>> GetById(Guid id)
        {
            var result = await _context.MstPages.AsNoTracking().Where(x=> x.PageId == id).ToListAsync();
            return result;
        }

        public async Task<int> Insert(MstPage model)
        {
            int result = 0;
            if (model != null)
            {
                _context.MstPages.Add(model);
                await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(MstPage model)
        {
            int result = 0;
            if (model != null)
            {
                _context.MstPages.Update(model);
                await _context.SaveChangesAsync();
            }
            return result;
        }
    }
}
