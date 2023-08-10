using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess
{
    public class ArchiveDestroyRepository : IArchiveDestroyRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;

        public ArchiveDestroyRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }
        public async Task<int> Delete(TrxArchiveDestroy model)
        {
            int result = 0;

            if (model.ArchiveDestroyId != Guid.Empty)
            {
                var data = _context.TrxArchiveDestroys.AsNoTracking().Where(x => x.ArchiveDestroyId == model.ArchiveDestroyId).FirstOrDefault();
                if (data != null)
                {
                    data.IsActive = false;
                    data.UpdatedBy = model.UpdatedBy;
                    data.UpdatedDate = DateTime.Now;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<TrxArchiveDestroy>(GlobalConst.Delete, (Guid)model!.UpdatedBy!, new List<TrxArchiveDestroy> { data }, new List<TrxArchiveDestroy> { });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
        public async Task<int> Submit(TrxArchiveDestroy model)
        {
            int result = 0;

            if (model.ArchiveDestroyId != Guid.Empty)
            {
                var data = await _context.TrxArchiveDestroys.AsNoTracking().FirstAsync(x => x.ArchiveDestroyId == model.ArchiveDestroyId);
                if (data != null)
                {
                    data.Note = model.Note;
                    data.ApproveLevel = model.ApproveLevel;
                    data.IsActive = model.IsActive;
                    data.StatusId = model.StatusId == (int)GlobalConst.STATUS.Approved ? (int)GlobalConst.STATUS.UsulMusnah : model.StatusId;
                    data.UpdatedBy = model.UpdatedBy;
                    data.UpdatedDate = model.UpdatedDate;
                    data.DestroySchedule = model.DestroySchedule;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<TrxArchiveDestroy>(GlobalConst.Update, (Guid)model!.UpdatedBy!, new List<TrxArchiveDestroy> { data }, new List<TrxArchiveDestroy> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
        public async Task<IEnumerable<TrxArchiveDestroy>> GetAll()
        {
            var results = await _context.TrxArchiveDestroys.Where(x => x.IsActive == true).ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxArchiveDestroys.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<TrxArchiveDestroy> GetById(Guid id)
        {
            var result = await _context.TrxArchiveDestroys.Include(x => x.ArchiveUnit.Company).AsNoTracking().FirstAsync(x => x.ArchiveDestroyId == id);
            return result;
        }
        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var User = AppUsers.CurrentUser(model.SessionUser);
            var result = await _context.TrxArchiveDestroys
                .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.SubSubjectClassification.SubjectClassification.Classification)
                .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room.Floor)
                .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                .Include(x => x.Status)
                .Where(x => x.IsActive == true && ( x.DestroyCode + x.DestroyName + x.Note + x.Status.Name).Contains(model.searchValue))
                .Where(x => x.IsArchiveActive == model.IsArchiveActive)
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitId == User.ArchiveUnitId))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveDestroyDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                .Where(model.advanceSearch!.Search)
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.ArchiveDestroyId,
                    x.DestroyCode,
                    x.DestroyName,
                    x.StatusId,
                    x.Note,
                    Color = x.Status.Color,
                    Status = x.Status.Name
                })
                .ToListAsync();

            return result;
        }
        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var User = AppUsers.CurrentUser(model.SessionUser);
            var result = await _context.TrxArchiveDestroys
                .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.SubSubjectClassification.SubjectClassification.Classification)
                .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room.Floor)
                .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                .Include(x => x.Status)
                .Where(x => x.IsActive == true && (x.DestroyCode + x.DestroyName + x.Note + x.Status.Name).Contains(model.searchValue))
                .Where(x => x.IsArchiveActive == model.IsArchiveActive)
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitId == User.ArchiveUnitId))
                .Where(model.advanceSearch!.Search)
                .CountAsync();

            return result;
        }
        public async Task<int> Insert(TrxArchiveDestroy model)
        {
            int result = 0;

            var count = await _context.TrxArchiveDestroys.CountAsync() + 1;

            if (model != null)
            {
                var archiveUnit = await _context.TrxArchiveUnits.FirstOrDefaultAsync(x => x.ArchiveUnitId == model.ArchiveUnitId);
                var company = await _context.MstCompanies.FirstOrDefaultAsync(x => x.CompanyId == archiveUnit!.CompanyId);

                model.IsActive = true;
                model.DestroyCode = $"DST.{count.ToString("D3")}/{DateTime.Now.Month.ToString("D2")}/{DateTime.Now.Year}";
                model.DocumentCode = $"PH.{count.ToString("D3")}-{company!.CompanyCode}/{archiveUnit!.ArchiveUnitCode}/{DateTime.Now.Month.ToString("D2")}/{model.CreatedDate.Year}";
                _context.TrxArchiveDestroys.Add(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxArchiveDestroy>(GlobalConst.New, (Guid)model!.CreatedBy!, new List<TrxArchiveDestroy> {  }, new List<TrxArchiveDestroy> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxArchiveDestroy> models)
        {
            bool result = false;
            if (models.Count() > 0)
            {
                await _context.AddRangeAsync(models);
                await _context.SaveChangesAsync();
                result = true;

                //Log
                if (result)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxArchiveDestroy>(GlobalConst.New, (Guid)models.FirstOrDefault()!.CreatedBy!, new List<TrxArchiveDestroy> {  }, models);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
        public async Task<int> Update(TrxArchiveDestroy model)
        {
            int result = 0;

            if (model != null && model.ArchiveDestroyId != Guid.Empty)
            {
                var data = await _context.TrxArchiveDestroys.AsNoTracking().FirstAsync(x => x.ArchiveDestroyId == model.ArchiveDestroyId);
                if (data != null)
                {
                    model.ArchiveUnit = null;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<TrxArchiveDestroy>(GlobalConst.Update, (Guid)model!.UpdatedBy!, new List<TrxArchiveDestroy> { data }, new List<TrxArchiveDestroy> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
    }
}
