using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly BksArditaDevContext _context;
        public UserRoleRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(IdxUserRole model)
        {
            int result = 0;

            if (model != null)
            {
                _context.IdxUserRoles.Remove(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<IdxUserRole>> GetAll()
        {
            var results = await _context.IdxUserRoles.ToListAsync();
            return results;
        }

        public async Task<IdxUserRole> GetById(Guid id)
        {
            var results = await _context.IdxUserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.UserRoleId == id);
            return results;
        }
        public async Task<IdxUserRole> GetByUserAndRoleId(Guid id, Guid role)
        {
            var results = await _context.IdxUserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id && x.RoleId == role);
            return results;
        }
        public async Task<IEnumerable<IdxUserRole>> GetIdxUserRoleByUserId(Guid id)
        {
            var result = await _context.IdxUserRoles
                .Include(x => x.User)
                .Include(x => x.Role)
               .AsNoTracking().Where(x => x.UserId == id).ToListAsync();
            return result;
        }
        public async Task<int> Insert(IdxUserRole model)
        {
            int result = 0;
            model.User = null;
            model.Role = null;

            if (model != null)
            {
                if (model.UserId != Guid.Empty && model.RoleId != Guid.Empty)
                {
                    var data = _context.IdxUserRoles.AsNoTracking().Where(x =>
                            x.UserId == model.UserId &&
                            x.RoleId == model.RoleId
                        );

                    if (data.Count() == 0)
                    {
                        _context.IdxUserRoles.Add(model);
                        result = await _context.SaveChangesAsync();
                    }
                }
            }
            return result;
        }

        public async Task<int> Update(IdxUserRole model)
        {
            model.User = null;
            model.Role = null;
            _context.IdxUserRoles.Update(model);
            return await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.IdxUserRoles
                    .Include(x => x.Role)
                    .Where(x => x.UserId == AppUsers.CurrentUser(model.SessionUser!).UserId)
                    .Where(x => (x.Role.Code + x.Role.Name).ToLower().Contains(model.searchValue!))
                    .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                    .Skip(model.skip).Take(model.pageSize)
                    .Select(x => new
                    {
                        x.RoleId,
                        x.Role.Code,
                        x.Role.Name,
                        Aktif = x.RoleId == AppUsers.CurrentUser(model.SessionUser!).RoleId
                    })
                    .ToListAsync();

            return result;
        }
        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var result = await _context.IdxUserRoles
                    .Include(x => x.Role)
                    .Where(x => x.UserId == AppUsers.CurrentUser(model.SessionUser!).UserId)
                    .Where(x => (x.Role.Code + x.Role.Name).ToLower().Contains(model.searchValue!))
                    .CountAsync();

            return result;
        }
    }
}
