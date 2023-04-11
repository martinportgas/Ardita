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
        public async Task<int> DeleteByRoleId(Guid roleId)
        {
            int result = 0;
            if (roleId != null)
            {
                _context.Database.ExecuteSqlRaw($" delete from dbo.MST_ROLE_PAGE where role_id='{roleId}'");
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
                var data = await _context.MstRolePages.AsNoTracking().Where(
                  x => x.RoleId == model.RoleId &&
                  x.PageId == model.PageId
                  ).ToListAsync();

                if (data.Count() == 0)
                {
                    _context.MstRolePages.Add(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<MstRolePage> model)
        {
            bool result = false;
            if (model.Count() > 0)
            {
                await _context.BulkInsertAsync(model);
                result = true;
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
