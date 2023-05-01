using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;

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
    public async Task<int> GetCount()
    {
        var results = await _context.VwArchiveRetentions.CountAsync();
        return results;
    }

    public async Task<IEnumerable<VwArchiveRetention>> GetArchiveRetentionByFilterModel(DataTableModel model)
    {
        IEnumerable<VwArchiveRetention> result;

        var propertyInfo = typeof(VwArchiveRetention).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(VwArchiveRetention).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.VwArchiveRetentions
            .Where(x => (x.TitleArchive + x.ArchiveNumber + x.ArchiveType + x.CreatorName + x.Status)
            .Contains(model.searchValue))
            .OrderBy(x => EF.Property<VwArchiveRetention>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }
        else
        {
            result = await _context.VwArchiveRetentions
            .Where(x => (x.TitleArchive + x.ArchiveNumber + x.ArchiveType + x.CreatorName + x.Status)
            .Contains(model.searchValue))
            .OrderByDescending(x => EF.Property<VwArchiveRetention>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }

        return result;
    }
}
