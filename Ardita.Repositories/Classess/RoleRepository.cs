using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ardita.Extensions;

namespace Ardita.Repositories.Classess
{
    public class RoleRepository : IRoleRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public RoleRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }
        public async Task<int> Delete(MstRole model)
        {
            int result = 0;

            if (model.RoleId != Guid.Empty)
            {
                var data = await _context.MstRoles.AsNoTracking().FirstOrDefaultAsync(x => x.RoleId == model.RoleId);
                if (data != null)
                {
                    model.IsActive = false;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    _context.MstRoles.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstRole>(GlobalConst.Delete, (Guid)model.UpdateBy!, new List<MstRole> { data }, new List<MstRole> {  });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstRole>> GetAll()
        {
            var result = await _context.MstRoles.AsNoTracking().Where(x => x.IsActive == true).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.MstRoles
                 //.Include(x => x.IdxUserRoles).ThenInclude(x => x.User)
                 .Where(
                     x => (x.Name).Contains(model.searchValue) &&
                     x.IsActive == true
                     )
                 .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                 .Skip(model.skip).Take(model.pageSize)
                 .Select(x => new {
                    x.RoleId,
                    x.Code,
                    x.Name
                    //UserId = x.IdxUserRoles.FirstOrDefault().UserId
                 })
                 .ToListAsync();

            return result;
        }

        public async Task<MstRole> GetById(Guid id)
        {
            
            var result = await _context.MstRoles.AsNoTracking().FirstOrDefaultAsync(x => x.RoleId == id);
            return result;
        }

        public async Task<int> GetCount(DataTableModel model)
        {
            var results = await _context.MstRoles
                .AsNoTracking()
                .Where($"(Name).Contains(@0) and IsActive = true", model.searchValue)
                .CountAsync();
            return results;
        }

        public async Task<int> Insert(MstRole model)
        {
            int result = 0;
            if (model != null)
            {
                var data = await _context.MstRoles.AsNoTracking().FirstOrDefaultAsync(x => x.Code == model.Code);
                model.IsActive = true;

                if (data != null)
                {
                    model.RoleId = data.RoleId;
                    model.UpdateBy = model.CreatedBy;
                    model.UpdateDate = DateTime.Now;
                    _context.MstRoles.Update(model);
                    result = await _context.SaveChangesAsync();
                }
                else
                {
                    _context.MstRoles.Add(model);
                    result = await _context.SaveChangesAsync();
                }

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstRole>(GlobalConst.New, model.CreatedBy, new List<MstRole> {  }, new List<MstRole> { model });
                    }
                    catch (Exception ex) { }
                }
            }

            return result;
        }

        public async Task<int> Update(MstRole model)
        {
            int result = 0;

            if (model.RoleId != Guid.Empty)
            {
                var data = await _context.MstRoles.AsNoTracking().FirstOrDefaultAsync(x => x.RoleId == model.RoleId);
                if (data != null)
                {
                    model.IsActive = true;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    _context.MstRoles.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstRole>(GlobalConst.Update, (Guid)model.UpdateBy!, new List<MstRole> { data }, new List<MstRole> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
    }
}
