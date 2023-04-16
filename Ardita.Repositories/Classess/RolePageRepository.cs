using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ardita.Repositories.Classess
{
    public class RolePageRepository : IRolePageRepository
    {
        private readonly BksArditaDevContext _context;
        public RolePageRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(IdxRolePage model)
        {
            int result = 0;
            if (model != null)
            {
                _context.IdxRolePages.Remove(model);
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

        public async Task<IEnumerable<IdxRolePage>> GetAll()
        {
            var result = await _context.IdxRolePages.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<IdxRolePage>> GetById(Guid id)
        {
            var result = await _context.IdxRolePages.AsNoTracking().Where(x => x.RolePageId == id).ToListAsync();
            return result;
        }

        public async Task<int> Insert(IdxRolePage model)
        {
            int result = 0;




            if (model != null)
            {
                var data = await _context.IdxRolePages.AsNoTracking().Where(
                  x => x.RoleId == model.RoleId &&
                  x.PageId == model.PageId
                  ).ToListAsync();

                if (data.Count() == 0)
                {
                    _context.IdxRolePages.Add(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<IdxRolePage> model)
        {
            bool result = false;
            if (model.Count() > 0)
            {
                await _context.BulkInsertAsync(model);
                result = true;
            }
            return result;
        }

        public async Task<int> Update(IdxRolePage model)
        {
            int result = 0;
            if (model != null)
            {
                _context.IdxRolePages.Update(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
    }
}
