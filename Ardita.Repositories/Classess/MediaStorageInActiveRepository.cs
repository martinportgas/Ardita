using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            .Where(x => x.IsActive == true && (x.MediaStorageInActiveCode + x.MediaStorageInActiveName + x.ArchiveYear + x.StatusId + x.SubSubjectClassification.SubSubjectClassificationName).Contains(model.searchValue))
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.MediaStorageInActiveId,
                    x.MediaStorageInActiveCode,
                    x.MediaStorageInActiveName,
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
            .Where(x => x.IsActive == true && (x.MediaStorageInActiveCode + x.MediaStorageInActiveName + x.ArchiveYear + x.StatusId + x.SubSubjectClassification.SubSubjectClassificationName).Contains(model.searchValue))
            .CountAsync();
    }
}
