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
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly BksArditaDevContext _context;
        public UserRoleRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(MstUserRole model)
        {
            int result = 0;

            if (model != null)
            {
                _context.MstUserRoles.Remove(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<MstUserRole>> GetAll()
        {
            var results = await _context.MstUserRoles.ToListAsync();
            return results;
        }

        public async Task<IEnumerable<MstUserRole>> GetById(Guid id)
        {
            var results = await _context.MstUserRoles.AsNoTracking().Where(x=>x.UserRoleId == id).ToListAsync();
            return results;
        }

        public async Task<int> Insert(MstUserRole model)
        {
            int result = 0;

            if (model != null)
            {
                _context.MstUserRoles.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(MstUserRole model)
        {
            int result = 0;

            if (model != null)
            {
                _context.MstUserRoles.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
    }
}
