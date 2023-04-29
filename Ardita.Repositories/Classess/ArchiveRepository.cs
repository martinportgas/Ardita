using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class ArchiveRepository : IArchiveRepository
{
    private readonly BksArditaDevContext _context;

    public ArchiveRepository(BksArditaDevContext context) => _context = context;

    public Task<int> Delete(TrxArchive model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxArchive>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TrxArchive>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<TrxArchive> result;

        var propertyInfo = typeof(TrxArchive).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(TrxArchive).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SubSubjectClassification)
                .Include(x => x.Creator)
                .Where(x => (x.TitleArchive).Contains(model.searchValue) && x.IsActive == true)
                .OrderBy(x => EF.Property<TrxArchive>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }
        else
        {
            result = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SubSubjectClassification)
                .Include(x => x.Creator)
                .Where(x => (x.TitleArchive).Contains(model.searchValue) && x.IsActive == true)
                .OrderByDescending(x => EF.Property<TrxArchive>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }

        return result;
    }

    public async Task<IEnumerable<TrxArchive>> GetByFilterModelForMonitoring(DataTableModel model)
    {
        IEnumerable<TrxArchive> result;

        var propertyInfo = typeof(TrxArchive).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(TrxArchive).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SubSubjectClassification)
                .Include(x => x.Creator)
                .Where(x => (x.TitleArchive).Contains(model.searchValue) && x.IsActive == true && x.SubSubjectClassification.TrxPermissionClassifications.FirstOrDefault().PositionId == model.PositionId)
                .OrderBy(x => EF.Property<TrxArchive>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }
        else
        {
            result = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SubSubjectClassification)
                .Include(x => x.Creator)
                .Where(x => (x.TitleArchive).Contains(model.searchValue) && x.IsActive == true && x.SubSubjectClassification.TrxPermissionClassifications.FirstOrDefault().PositionId == model.PositionId)
                .OrderByDescending(x => EF.Property<TrxArchive>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }

        return result;
    }

    public async Task<TrxArchive> GetById(Guid id)
    {
        var result = await _context.TrxArchives.AsNoTracking()
            .Include(x => x.Gmd)
            .Include(x => x.SubSubjectClassification)
            .Include(x => x.SecurityClassification)
            .Include(x => x.Creator)
            .Include(x => x.TrxFileArchiveDetails)
            .Include(x => x.TrxMediaStorageDetails)
            .ThenInclude(x => x.MediaStorage)
            .ThenInclude(x => x.Row)
            .ThenInclude(x => x.Level)
            .ThenInclude(x => x.Rack)
            .ThenInclude(x => x.Room)
            .ThenInclude(x => x.Floor)
            .ThenInclude(x => x.ArchiveUnit)
            .Include(x => x.TrxMediaStorageDetails)
            .ThenInclude(x => x.MediaStorage)
            .ThenInclude(x => x.TypeStorage)

            .Where(x => x.ArchiveId == id && x.IsActive == true)
            .FirstAsync();

        

        return result;
    }

    public async Task<int> GetCount() => await _context.TrxArchives.CountAsync(x => x.IsActive == true);
    public async Task<int> GetCountForMonitoring(Guid? PositionId)
    {
       return await _context
            .TrxArchives
            .Include(x => x.Gmd)
            .Include(x => x.SubSubjectClassification)
            .Include(x => x.Creator)
            .CountAsync(x => x.IsActive == true 
            && x.SubSubjectClassification.TrxPermissionClassifications.FirstOrDefault().PositionId == PositionId);
    }

    public Task<int> Insert(TrxArchive model)
    {
        throw new NotImplementedException();
    }

    public Task<int> Update(TrxArchive model)
    {
        throw new NotImplementedException();
    }
}
