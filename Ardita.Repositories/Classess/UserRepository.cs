using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class UserRepository : IUserRepository
    {
        private readonly BksArditaDevContext _context;
        public UserRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(MstUser model)
        {
            int result = 0;

            if (model.UserId != Guid.Empty)
            {
                var data = await _context.MstUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == model.UserId);
                if (data != null)
                {
                    model.IsActive = false;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    model.Password = data.Password;
                    model.EmployeeId = data.EmployeeId;
                    _context.MstUsers.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstUser>> GetAll()
        {
            var result = await _context.MstUsers
                .Include(x => x.Employee.Position)
                .Include(x => x.IdxUserRoles)
                .ThenInclude(x => x.Role)
                
                .AsNoTracking()
                .Where(x => x.IsActive == true).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MstUser>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<MstUser> result;

            var propertyInfo = typeof(MstUser).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(MstUser).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.MstUsers
                 .Include(x => x.Employee.Position)
                .Include(x => x.IdxUserRoles)
                .ThenInclude(x => x.Role)
                .Where(
                    x => (x.Username).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderBy(x => EF.Property<MstUser>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.MstUsers
                    .Include(x => x.Employee.Position)
                .Include(x => x.IdxUserRoles)
                .ThenInclude(x => x.Role)
                .Where(
                    x => (x.Username).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderByDescending(x => EF.Property<MstUser>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<MstUser> GetById(Guid id)
        {
            var result = await _context.MstUsers
                .Include(x => x.Employee.Position)
                .Include(x => x.IdxUserRoles)
                .ThenInclude(x => x.Role)

                .AsNoTracking().FirstAsync(x => x.UserId == id);
            return result;
        }

        public async Task<int> GetCount()
        {
            var results = await _context.MstUsers.AsNoTracking().Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<int> Insert(MstUser model)
        {
            int result = 0;
            if (model != null)
            {
                var data = await _context.MstUsers.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId);
                model.IsActive = true;

                if (data != null)
                {

                    model.UserId = data.UserId;
                    model.UpdateBy = model.CreatedBy;
                    model.UpdateDate = DateTime.Now;
                    _context.MstUsers.Update(model);
                    result = await _context.SaveChangesAsync();
                }
                else
                {
                    _context.MstUsers.Add(model);
                    result = await _context.SaveChangesAsync();
                }
            }

            return result;
        }

        public async Task<bool> InsertBulk(List<MstUser> users)
        {
            bool result = false;
            if (users.Count() > 0)
            {
                await _context.AddRangeAsync(users);
                await _context.SaveChangesAsync();
                result = true;
            }
            return result;
        }

        public async Task<int> Update(MstUser model)
        {
            int result = 0;

            if (model.UserId != Guid.Empty)
            {
                var data = await _context.MstUsers.AsNoTracking().FirstAsync(x => x.UserId == model.UserId);
                if (data != null)
                {
                    model.IsActive = true;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    _context.MstUsers.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
