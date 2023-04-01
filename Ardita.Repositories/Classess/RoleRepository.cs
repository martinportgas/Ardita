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
    public class RoleRepository : IRoleRepository
    {
        private readonly BksArditaDevContext _context;
        public RoleRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public Task<int> Delete(MstRole model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MstRole>> GetAll()
        {
            var result = await _context.MstRoles.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MstRole>> GetById(Guid id)
        {
            
            var result = await _context.MstRoles.Where(x => x.RoleId == id).ToListAsync();
            return result;
        }

        public async Task<int> Insert(MstRole model)
        {
            int result = 0;

            if (model != null)
            {
                _context.MstRoles.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(MstRole model)
        {
            int result = 0;

            if (model != null && model.RoleId != Guid.Empty)
            {
                var data = await _context.MstRoles.Where(x => x.RoleId == model.RoleId).ToListAsync();
                if (data != null)
                {
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
