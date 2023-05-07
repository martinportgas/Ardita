using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<IdxUserRole>> GetById(Guid id)
        {
            var results = await _context.IdxUserRoles.AsNoTracking().Where(x => x.UserRoleId == id).ToListAsync();
            return results;
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
            int result = 0;


            return result;
        }
    }
}
