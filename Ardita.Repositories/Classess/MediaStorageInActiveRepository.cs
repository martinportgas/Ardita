using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess;

public class MediaStorageInActiveRepository : IMediaStorageInActiveRepository
{
    private readonly BksArditaDevContext _context;
    public MediaStorageInActiveRepository(BksArditaDevContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        return await _context.TrxMediaStorageInActives
            .Include(x => x.SubSubjectClassification.Creator)
            .Include(x => x.TypeStorage.ArchiveUnit)
            .Include(x => x.Status)
            .Where(x => x.IsActive == true && (x.MediaStorageInActiveCode + x.ArchiveYear + x.StatusId + x.SubSubjectClassification.SubSubjectClassificationName).Contains(model.searchValue))
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.MediaStorageInActiveId,
                    x.MediaStorageInActiveCode,
                    x.SubSubjectClassification.SubSubjectClassificationName,
                    x.SubSubjectClassification.Creator.CreatorName,
                    x.TypeStorage.TypeStorageName,
                    x.TypeStorage.ArchiveUnit.ArchiveUnitName,
                    x.ArchiveYear,
                    x.StatusId,
                    x.Status.Color,
                    Status = x.Status.Name
                })
                .ToListAsync();
    }

    public async Task<TrxMediaStorageInActive> GetById(Guid id)
    {
        var data = await _context.TrxMediaStorageInActives
            .Include(g => g.GmdDetail)
            .Include(d => d.TrxMediaStorageInActiveDetails)
                .ThenInclude(a => a.Archive)
                .ThenInclude(c => c.Creator)
            .Include(x => x.TrxMediaStorageInActiveDetails)
                .ThenInclude(x => x.SubTypeStorage)
            .Include(s => s.SubSubjectClassification.Creator)
            .Include(t => t.TypeStorage)
                .ThenInclude(a => a.ArchiveUnit)
            .Include(r => r.Row!.Level!.Rack!.Room!.Floor)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.MediaStorageInActiveId == id);
        return data!;
    }

    public async Task<int> GetCountByFilterModel(DataTableModel model)
    {
        return await _context.TrxMediaStorageInActives
            .Include(x => x.SubSubjectClassification)
            .Include(x => x.Status)
            .Where(x => x.IsActive == true && (x.MediaStorageInActiveCode + x.ArchiveYear + x.StatusId + x.SubSubjectClassification.SubSubjectClassificationName).Contains(model.searchValue))
            .CountAsync();
    }

    public async Task<IEnumerable<object>> GetDetailArchive(Guid id)
    {
        var data = await _context.TrxArchives
            .Include(x => x.Creator.ArchiveUnit)
            .Include(x => x.SubSubjectClassification)
            .AsNoTracking()
            .Where(x => x.ArchiveId == id).ToListAsync();
            
        return data;
    }

    public async Task<TrxMediaStorageInActiveDetail> GetDetailByArchiveId(Guid id)
    {
        var result = await _context.TrxMediaStorageInActiveDetails
            .Include(x => x.MediaStorageInActive.TypeStorage.ArchiveUnit)
            .FirstOrDefaultAsync(x=>x.ArchiveId == id);
        return result;
    }

    public async Task<IEnumerable<VwArchiveRent>> GetDetailByArchiveIdAndSort(Guid id, int sort)
    {
        var results = await _context.VwArchiveRents
            .Where(x => x.MediaStorageInActiveId == id && x.Sort == sort)
            .ToListAsync();
            
        return results;

    }

    public async Task<int> Insert(TrxMediaStorageInActive model, List<TrxMediaStorageInActiveDetail> detail)
    {
        int result = 0;

        if (model is not null)
        {
            foreach (var e in _context.ChangeTracker.Entries())
            {
                e.State = EntityState.Detached;
            }
            model.IsActive = true;

            _context.Entry(model).State = EntityState.Added;
            await _context.SaveChangesAsync();

            if (detail.Any())
            {
                foreach (var item in detail)
                {
                    item.MediaStorageInActiveId = model.MediaStorageInActiveId;
                    _context.TrxMediaStorageInActiveDetails.Add(item);
                    result += await _context.SaveChangesAsync();
                }
            }
        }

        return result;
    }
    public async Task<int> Delete(Guid ID)
    {
        int result = 0;
        var detail = await _context.TrxMediaStorageInActiveDetails.Where(x => x.MediaStorageInActiveId == ID).ToListAsync();
        if (detail.Any())
        {
            _context.TrxMediaStorageInActiveDetails.RemoveRange(detail);
            await _context.SaveChangesAsync();
        }
        var main = await _context.TrxMediaStorageInActives.FirstOrDefaultAsync(x => x.MediaStorageInActiveId == ID);
        if(main != null)
        {
            _context.TrxMediaStorageInActives.Remove(main);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }
    public async Task<int> Update(TrxMediaStorageInActive model, List<TrxMediaStorageInActiveDetail> detail)
    {
        int result = 0;

        if (model is not null)
        {
            foreach (var e in _context.ChangeTracker.Entries())
            {
                e.State = EntityState.Detached;
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var oldDetail = await _context.TrxMediaStorageInActiveDetails.Where(x => x.MediaStorageInActiveId == model.MediaStorageInActiveId).ToListAsync();
            if(oldDetail.Any())
            {
                _context.TrxMediaStorageInActiveDetails.RemoveRange(oldDetail);
                await _context.SaveChangesAsync();
            }

            if (detail.Any())
            {
                foreach (var item in detail)
                {
                    item.MediaStorageInActiveId = model.MediaStorageInActiveId;
                    _context.TrxMediaStorageInActiveDetails.Add(item);
                    result += await _context.SaveChangesAsync();
                }
            }
        }

        return result;
    }
}
