using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess;

public class ArchiveRetentionRepository : IArchiveRetentionRepository
{
    private readonly BksArditaDevContext _context;

    public ArchiveRetentionRepository(BksArditaDevContext context) => _context = context;
    public async Task<IEnumerable<VwArchiveRetention>> GetAll()
    {
        var results = await _context.VwArchiveRetentions.ToListAsync();
        return results;
    }
    public async Task<IEnumerable<VwArchiveRetentionInActive>> GetInActiveAll()
    {
        var results = await _context.VwArchiveRetentionInActives.ToListAsync();
        return results;
    }
    public async Task<int> GetCount()
    {
        var results = await _context.VwArchiveRetentions.CountAsync();
        return results;
    }

    public async Task<IEnumerable<object>> GetArchiveRetentionByFilterModel(DataTableModel model)
    {
        var User = AppUsers.CurrentUser(model.SessionUser);
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("id-ID");
        var result = (bool)model.IsArchiveActive ? await _context.VwArchiveRetentions
            .Where(x => (x.TitleArchive + x.ArchiveNumber + x.ArchiveType + x.CreatorName + x.Status + x.RetentionDateArchive.ToString()).Contains(model.searchValue))
            .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitId == User.ArchiveUnitId))
            .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
            .Where(model.advanceSearch!.Search)
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
            .Skip(model.skip).Take(model.pageSize)
            .Select(x => new
            {
                x.ArchiveId,
                x.ArchiveType,
                x.CreatorName,
                x.Status,
                x.ArchiveNumber,
                x.TitleArchive,
                RetentionDateArchive = x.RetentionDateArchive.ToString()
            })
            .ToListAsync() : await _context.VwArchiveRetentionInActives
            .Where(x => (x.TitleArchive + x.ArchiveNumber + x.ArchiveType + x.CreatorName + x.Status + x.RetentionDateArchive.ToString()).Contains(model.searchValue))
            .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitId == User.ArchiveUnitId))
            .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
            .Where(model.advanceSearch!.Search)
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
            .Skip(model.skip).Take(model.pageSize)
            .Select(x => new
            {
                x.ArchiveId,
                x.ArchiveType,
                x.CreatorName,
                x.Status,
                x.ArchiveNumber,
                x.TitleArchive,
                RetentionDateArchive = x.RetentionDateArchive.ToString()
            })
            .ToListAsync();

        return result;
    }
    public async Task<int> GetCountArchiveRetentionByFilterModel(DataTableModel model)
    {
        var User = AppUsers.CurrentUser(model.SessionUser);
        var result = (bool)model.IsArchiveActive ? await _context.VwArchiveRetentions
            .Where(x => (x.TitleArchive + x.ArchiveNumber + x.ArchiveType + x.CreatorName + x.Status + x.RetentionDateArchive.ToString()).Contains(model.searchValue))
            .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitId == User.ArchiveUnitId))
            .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
            .Where(model.advanceSearch!.Search)
            .CountAsync() : await _context.VwArchiveRetentionInActives
            .Where(x => (x.TitleArchive + x.ArchiveNumber + x.ArchiveType + x.CreatorName + x.Status + x.RetentionDateArchive.ToString()).Contains(model.searchValue))
            .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitId == User.ArchiveUnitId))
            .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
            .Where(model.advanceSearch!.Search)
            .CountAsync();

        return result;
    }
}
