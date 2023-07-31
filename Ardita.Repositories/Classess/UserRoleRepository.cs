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
            var results = await _context.IdxUserRoles.Include(x => x.Role).Include(x => x.User.Employee.Position).ToListAsync();
            return results;
        }

        public async Task<IdxUserRole> GetById(Guid id)
        {
            var results = await _context.IdxUserRoles
                    .Include(x => x.Role)
                    .Include(x => x.ArchiveUnit)
                    .Include(x => x.Creator)
                    .AsNoTracking().FirstOrDefaultAsync(x => x.UserRoleId == id);
            return results;
        }
        public async Task<IdxUserRole> GetByUserAndRoleId(Guid id, Guid role, Guid archiveUnit, Guid creator)
        {
            var results = await _context.IdxUserRoles.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == id && x.RoleId == role && (archiveUnit == Guid.Empty ? x.ArchiveUnitId == null : x.ArchiveUnitId == archiveUnit) && (creator == Guid.Empty ? x.CreatorId == null : x.CreatorId == creator));
            return results;
        }
        public async Task<IEnumerable<IdxUserRole>> GetIdxUserRoleByUserId(Guid id)
        {
            var result = await _context.IdxUserRoles
                .Include(x => x.User)
                .Include(x => x.Role)
                .Include(x => x.ArchiveUnit)
                .Include(x => x.Creator)
               .AsNoTracking().Where(x => x.UserId == id).ToListAsync();
            return result;
        }
        public async Task<int> Insert(IdxUserRole model)
        {
            int result = 0;
            model.User = null;
            model.Role = null;
            model.ArchiveUnit = null;
            model.Creator = null;

            if (model != null)
            {
                if (model.UserId != Guid.Empty && model.RoleId != Guid.Empty)
                {
                    var data = _context.IdxUserRoles.AsNoTracking().Where(x =>
                            x.UserId == model.UserId &&
                            x.RoleId == model.RoleId &&
                            x.ArchiveUnitId == model.ArchiveUnitId &&
                            x.CreatorId == model.CreatorId
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
                    .Include(x => x.ArchiveUnit)
                    .Include(x => x.Creator)
                    .Where(x => x.UserId == AppUsers.CurrentUser(model.SessionUser!).UserId)
                    .Where(x => (x.Role.Code + x.Role.Name).ToLower().Contains(model.searchValue!))
                    .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                    .Skip(model.skip).Take(model.pageSize)
                    .Select(x => new
                    {
                        x.UserRoleId,
                        x.RoleId,
                        x.Role.Code,
                        x.Role.Name,
                        x.ArchiveUnit.ArchiveUnitName,
                        x.Creator.CreatorName,
                        Aktif = x.RoleId == AppUsers.CurrentUser(model.SessionUser!).RoleId && x.ArchiveUnitId == (AppUsers.CurrentUser(model.SessionUser!).ArchiveUnitId == Guid.Empty ? null : AppUsers.CurrentUser(model.SessionUser!).ArchiveUnitId) && x.CreatorId == (AppUsers.CurrentUser(model.SessionUser!).CreatorId == Guid.Empty ? null : AppUsers.CurrentUser(model.SessionUser!).CreatorId)
                    })
                    .ToListAsync();

            return result;
        }
        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var result = await _context.IdxUserRoles
                    .Include(x => x.Role)
                    .Include(x => x.ArchiveUnit)
                    .Include(x => x.Creator)
                    .Where(x => x.UserId == AppUsers.CurrentUser(model.SessionUser!).UserId)
                    .Where(x => (x.Role.Code + x.Role.Name).ToLower().Contains(model.searchValue!))
                    .CountAsync();

            return result;
        }
    }
}
