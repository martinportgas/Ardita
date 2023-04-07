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
    public class RolePageRepository : IRolePageRepository
    {
        private readonly BksArditaDevContext _context;
        public RolePageRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(MstRolePage model)
        {
            int result = 0;
            if (model != null)
            {
                _context.MstRolePages.Remove(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
                
        }

        public async Task<IEnumerable<MstRolePage>> GetAll()
        {
            var result = await _context.MstRolePages.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MstRolePage>> GetById(Guid id)
        {
            var result = await _context.MstRolePages.AsNoTracking().Where(x => x.RolePageId == id).ToListAsync();
            return result;
        }

        public async Task<int> Insert(MstRolePage model)
        {
            int result = 0;
            if (model != null)
            {
                _context.MstRolePages.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(MstRolePage model)
        {
            int result = 0;
            if (model != null)
            {
                _context.MstRolePages.Update(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
    }
}
