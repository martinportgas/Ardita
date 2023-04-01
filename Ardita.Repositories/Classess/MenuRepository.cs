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
    public class MenuRepository : IMenuRepository
    {
        private readonly BksArditaDevContext _context;
        public MenuRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public Task<int> Delete(MstMenu model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MstMenu>> GetAll()
        {
            var result = await _context.MstMenus.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MstMenu>> GetById(Guid id)
        {
            var result = await _context.MstMenus.Where(x => x.MenuId == id).ToListAsync();
            return result;
        }

        public async Task<int> Insert(MstMenu model)
        {
            int result = 0;
            var menus = await _context.MstMenus.Where(x => x.MenuId == model.MenuId).ToListAsync();
            if (menus.Count == 0)
            {
                _context.MstMenus.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(MstMenu model)
        {
            int result = 0;

            _context.Update(model);
            result = await _context.SaveChangesAsync();
            return result;
        }
    }
}
