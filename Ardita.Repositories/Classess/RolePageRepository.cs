using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ardita.Repositories.Classess
{
    public class RolePageRepository : IRolePageRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public RolePageRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }

        public async Task<int> Delete(IdxRolePage model)
        {
            int result = 0;
            if (model != null)
            {
                _context.IdxRolePages.Remove(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<IdxRolePage>(GlobalConst.Delete, (Guid)model.UpdateBy!, new List<IdxRolePage> { model }, new List<IdxRolePage> {  });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;

        }
        public async Task<int> DeleteByRoleId(Guid roleId)
        {
            int result = 0;
            if (roleId != null)
            {
                var dataOld = await _context.IdxRolePages.Where(x => x.RoleId == roleId).ToListAsync();
                _context.Database.ExecuteSqlRaw($" delete from dbo.IDX_ROLE_PAGE where role_id='{roleId}'");
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<IdxRolePage>(GlobalConst.Delete, (Guid)dataOld.FirstOrDefault()!.UpdateBy!, dataOld, new List<IdxRolePage> { });
                    }
                    catch (Exception ex) { }
                }
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

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<IdxRolePage>(GlobalConst.New, model.CreatedBy, new List<IdxRolePage> { }, new List<IdxRolePage> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<IdxRolePage> model)
        {
            bool result = false;
            if (model.Count() > 0)
            {
                await _context.AddRangeAsync(model);
                await _context.SaveChangesAsync();
                result = true;

                //Log
                if (result)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<IdxRolePage>(GlobalConst.New, model.FirstOrDefault()!.CreatedBy, new List<IdxRolePage> { }, model);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<int> Update(IdxRolePage model)
        {
            int result = 0;
            if (model != null)
            {
                var dataOld = await _context.IdxRolePages.FirstOrDefaultAsync(x => x.RolePageId == model.RolePageId);
                if(dataOld != null)
                {
                    _context.IdxRolePages.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<IdxRolePage>(GlobalConst.Update, (Guid)model.UpdateBy!, new List<IdxRolePage> { dataOld }, new List<IdxRolePage> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
    }
}
