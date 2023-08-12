using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ardita.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using NPOI.SS.Formula.Functions;

namespace Ardita.Repositories.Classess
{
    public class UserRepository : IUserRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public UserRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
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

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstUser>(GlobalConst.Delete, model.CreatedBy, new List<MstUser> { data }, new List<MstUser> { model });
                        }
                        catch (Exception ex) { }
                    }
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
                .Where(x => x.IsActive == true)
                .Where(x => x.Employee.IsActive == true)
                .Where(x => x.Employee.Position.IsActive == true)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.MstUsers
                  .Include(x => x.Employee.Position)
                  .Include(x => x.IdxUserArchiveUnits)
                  .ThenInclude(x => x.ArchiveUnit)
                 .Where(
                             x => (x.Username).Contains(model.searchValue) &&
                             x.IsActive == true
                    )
                 .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                 .Skip(model.skip).Take(model.pageSize)
                 .Select(x => new { 
                    x.UserId,
                    x.Username,
                    EmployeeName = x.Employee.Name,
                    PositionName = x.Employee.Position.Name,
                    UnitArchive = x.IdxUserArchiveUnits.FirstOrDefault().ArchiveUnit.ArchiveUnitName

                 })
                 .ToListAsync();

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

        public async Task<int> GetCount(DataTableModel model)
        {
            var results = await _context.MstUsers
                .AsNoTracking()
                .Where($"(Username).Contains(@0) and IsActive = true", model.searchValue)
                .CountAsync();
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

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstUser>(GlobalConst.New, model.CreatedBy, new List<MstUser> { data }, new List<MstUser> { model });
                    }
                    catch (Exception ex) { }
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

                //Log
                if (result)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstUser>(GlobalConst.New, users.FirstOrDefault().CreatedBy, new List<MstUser> {  }, users);
                    }
                    catch (Exception ex) { }
                }
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

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstUser>(GlobalConst.Update, (Guid)model.UpdateBy!, new List<MstUser> { data }, new List<MstUser> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
        public async Task<int> ChangePassword(MstUser model)
        {
            int result = 0;

            if (model.UserId != Guid.Empty)
            {
                var data = await _context.MstUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == model.UserId);
                if (data != null)
                {
                    model.IsActive = true;
                    model.Username = data.Username;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    model.PasswordLastChanged = DateTime.Now;
                    model.EmployeeId = data.EmployeeId;
                    
                    _context.MstUsers.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstUser>(GlobalConst.Update, (Guid)model.UpdateBy!, new List<MstUser> { data }, new List<MstUser> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
        public async Task<bool> FindPasswordByUsername(Guid Id, string password) 
        {
            var data = await _context.MstUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == Id && x.Password == password);
            if (data != null)
                return true;
            else
                return false;
        }
    }
}
