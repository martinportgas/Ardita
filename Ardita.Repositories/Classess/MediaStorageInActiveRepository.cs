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
            .Include(x => x.SubSubjectClassification)
            .Include(x => x.Status)
            .Where(x => x.IsActive == true && (x.MediaStorageInActiveCode + x.ArchiveYear + x.StatusId + x.SubSubjectClassification.SubSubjectClassificationName).Contains(model.searchValue))
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.MediaStorageInActiveId,
                    x.MediaStorageInActiveCode,
                    x.SubSubjectClassification.SubSubjectClassificationName,
                    x.StatusId,
                    x.Status.Color,
                    Status = x.Status.Name
                })
                .ToListAsync();
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

    public async Task<IEnumerable<object>> GetDetailByArchiveIdAndSort(Guid id, int sort)
    {
        var results = await _context.VwArchiveRents
            .Where(x => x.ArchiveId == id || x.Sort == sort)
            .ToListAsync();
            
        return results;

    }
}
