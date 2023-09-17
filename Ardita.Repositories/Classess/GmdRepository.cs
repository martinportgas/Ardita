using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Dynamic.Core;
using Ardita.Extensions;
using Ardita.Report;
using Ardita.Models;

namespace Ardita.Repositories.Classess;

public class GmdRepository : IGmdRepository
{
    private readonly BksArditaDevContext _context;
    private readonly ILogChangesRepository _logChangesRepository;
    public GmdRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
    {
        _context = context;
        _logChangesRepository = logChangesRepository;
    }
    public async Task<int> Delete(MstGmd model)
    {
        int result = 0;

        if (model.GmdId != Guid.Empty)
        {
            var data = await _context.MstGmds.AsNoTracking().FirstAsync(x => x.GmdId == model.GmdId);
            if (data != null)
            {
                model.IsActive = false;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                _context.Update(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstGmd>(GlobalConst.Delete, (Guid)model.UpdatedBy!, new List<MstGmd> { data }, new List<MstGmd> {  });
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }

    public async Task<IEnumerable<MstGmd>> GetAll(string par = " 1=1 ") => await _context.MstGmds.Where(x => x.IsActive == true).Where(par).AsNoTracking().ToListAsync();
    public async Task<IEnumerable<object>> GetGMDGroupByArchiveCount(GlobalSearchModel search, string par = " 1=1 ")
    {
        try
        {
            return await _context.MstGmds
                        .AsNoTracking()
                        .Where(x => x.IsActive == true)
                        .Select(y => new
                        {
                            name = y.GmdName,
                            totalArchive = (_context.TrxArchives
                            .Include(x => x.SubSubjectClassification)
                            .Include(x => x.SecurityClassification)
                            .Include(x => x.Creator)
                            .Include(x => x.ArchiveOwner)
                            .Include(x => x.ArchiveType)
                            .Where(x => x.IsActive == true)
                            .Where(x => x.SubSubjectClassification.IsActive == true)
                            .Where(x => x.SecurityClassification.IsActive == true)
                            .Where(x => x.Creator.IsActive == true)
                            .Where(x => x.ArchiveOwner.IsActive == true)
                            .Where(x => x.ArchiveType.IsActive == true)
                            .Where(x => x.GmdId == y.GmdId)
                            .Where(x => search.StatusId == null ? true : x.StatusId == search.StatusId)
                            .Where(x => search.IsArchiveActive == null ? true : x.IsArchiveActive == search.IsArchiveActive)
                            .Where(x => search.ArchiveUnitId == null ? true : x.Creator.ArchiveUnitId == search.ArchiveUnitId)
                            .Where(x => search.CreatorId == null ? true : x.CreatorId == search.CreatorId)
                            .Count())
                        })
                        .OrderByDescending(x => x.totalArchive)
                        .ToListAsync();
        }
        catch(Exception ex)
        {
            throw;
        }
    }

    public async Task<IEnumerable<MstGmdDetail>> GetDetailByGmdId(Guid Id) => await _context.MstGmdDetails.Where(x => x.GmdId == Id).AsNoTracking().ToListAsync();
    
    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        var result = await _context.MstGmds
                .Where(x => (x.GmdCode + x.GmdName).Contains(model.searchValue) && x.IsActive)
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.GmdId,
                    x.GmdCode,
                    x.GmdName
                })
                .ToListAsync();

        return result;
    }
    public async Task<int> GetCountByFilterModel(DataTableModel model)
    {
        var result = await _context.MstGmds
                .Where(x => (x.GmdCode + x.GmdName).Contains(model.searchValue) && x.IsActive)
                .CountAsync();

        return result;
    }

    public async Task<IEnumerable<MstGmd>> GetById(Guid id) 
        => await _context.MstGmds
        .Include(x => x.MstGmdDetails)
        .Where(x => x.GmdId== id)
        .AsNoTracking()
        .ToListAsync();

    public async Task<int> GetCount() => await _context.MstGmds.CountAsync(x => x.IsActive == true);

    public async Task<int> Insert(MstGmd model, List<MstGmdDetail> details)
    {
        int result = 0;
        List<MstGmdDetail> gmdDetails = new();


        if (model != null)
        {
            model.IsActive = true;
            _context.MstGmds.Add(model);
            result = await _context.SaveChangesAsync();

            foreach (var item in details)
            {
                item.CreatedDate = model.CreatedDate;
                item.CreatedBy = model.CreatedBy;
                item.GmdId = model.GmdId;

                gmdDetails.Add(item);
            }

            _context.MstGmdDetails.AddRange(gmdDetails);
            result += await _context.SaveChangesAsync();

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstGmd>(GlobalConst.New, model.CreatedBy, new List<MstGmd> {  }, new List<MstGmd> { model });
                    await _logChangesRepository.CreateLog<MstGmdDetail>(GlobalConst.New, model.CreatedBy, new List<MstGmdDetail> {  }, details);
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }

    public async Task<bool> InsertBulk(List<MstGmd> mstGmds)
    {
        bool result = false;
        if (mstGmds.Count() > 0)
        {
            await _context.AddRangeAsync(mstGmds);
            await _context.SaveChangesAsync();
            result = true;

            //Log
            if (result)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstGmd>(GlobalConst.New, mstGmds.FirstOrDefault()!.CreatedBy, new List<MstGmd> { }, mstGmds);
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }

    public async Task<int> Update(MstGmd model, List<MstGmdDetail> details)
    {
        int result = 0;
        List<MstGmdDetail> gmdDetails = new();
        List<MstGmdDetail> oldGmdDetails = new();

        if (model != null && model.GmdId != Guid.Empty)
        {
            var data = await _context.MstGmds.AsNoTracking().FirstAsync(x => x.GmdId == model.GmdId);
            if (data != null)
            {
                //Insert Header
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                model.IsActive = data.IsActive;
                _context.Update(model);
                result = await _context.SaveChangesAsync();

                //Insert Detail
                oldGmdDetails = await _context.MstGmdDetails.AsNoTracking().Where(x => x.GmdId == model.GmdId && x.TrxTypeStorageDetails.FirstOrDefault() == null && x.MstSubTypeStorageDetails.FirstOrDefault() == null).ToListAsync();
                if (oldGmdDetails.Any())
                {
                    _context.MstGmdDetails.RemoveRange(oldGmdDetails);
                    result += await _context.SaveChangesAsync();
                }


                foreach (var item in details)
                {
                    var count = await _context.MstGmdDetails.AsNoTracking().Where(x => x.GmdId == model.GmdId && x.Name == item.Name && x.Unit == item.Unit).CountAsync();
                    if (count == 0)
                    {
                        item.CreatedDate = model.CreatedDate;
                        item.CreatedBy = model.CreatedBy;
                        item.GmdId = model.GmdId;

                        gmdDetails.Add(item);
                    }
                }

                _context.MstGmdDetails.AddRange(gmdDetails);
                result += await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstGmd>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<MstGmd> { data }, new List<MstGmd> { model });
                        await _logChangesRepository.CreateLog<MstGmdDetail>(GlobalConst.Update, (Guid)model.UpdatedBy!, oldGmdDetails, details);
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }

    public async Task<MstGmdDetail> GetDetailById(Guid Id) => await _context.MstGmdDetails.AsNoTracking().FirstAsync(x => x.GmdDetailId == Id);

    public async Task<IEnumerable<MstGmdDetail>> GetAllDetail() => await _context.MstGmdDetails.AsNoTracking().ToListAsync();
}
