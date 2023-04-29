using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class ArchiveUnitRepository : IArchiveUnitRepository
{
    private readonly BksArditaDevContext _context;

    public ArchiveUnitRepository(BksArditaDevContext context) => _context = context;

    public async Task<int> Delete(TrxArchiveUnit model)
    {
        int result = 0;

        if (model.ArchiveUnitId != Guid.Empty)
        {
            var data = await _context.TrxArchiveUnits.AsNoTracking().FirstAsync(x => x.ArchiveUnitId== model.ArchiveUnitId);
            if (data != null)
            {
                data.IsActive = false;
                data.UpdatedDate = model.UpdatedDate;
                data.UpdatedBy = model.UpdatedBy;
                _context.TrxArchiveUnits.Update(data);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }

    public async Task<IEnumerable<TrxArchiveUnit>> GetAll() => await _context.TrxArchiveUnits.Where(x => x.IsActive == true).ToListAsync();

    public async Task<IEnumerable<TrxArchiveUnit>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<TrxArchiveUnit> result;

        var propertyInfo = typeof(TrxArchiveUnit).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(TrxArchiveUnit).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.TrxArchiveUnits
                .Where(x => (x.ArchiveUnitCode + x.ArchiveUnitName).Contains(model.searchValue) && x.IsActive == true)
                .OrderBy(x => EF.Property<TrxArchiveUnit>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }
        else
        {
            result = await _context.TrxArchiveUnits
                .Where(x => (x.ArchiveUnitCode + x.ArchiveUnitName).Contains(model.searchValue) && x.IsActive == true)
                .OrderByDescending(x => EF.Property<TrxArchiveUnit>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }
        return result;
    }

    public async Task<IEnumerable<TrxArchiveUnit>> GetById(Guid id) => await _context.TrxArchiveUnits.Where(x => x.ArchiveUnitId == id).ToListAsync();

    public async Task<int> GetCount() => await _context.TrxArchiveUnits.CountAsync(x => x.IsActive == true);

    public async Task<int> Insert(TrxArchiveUnit model)
    {
        int result = 0;

        if (model != null)
        {
            model.IsActive = true;
            _context.TrxArchiveUnits.Add(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<int> Update(TrxArchiveUnit model)
    {
        int result = 0;

        if (model != null && model.ArchiveUnitId != Guid.Empty)
        {
            var data = await _context.TrxArchiveUnits.AsNoTracking().FirstAsync(x => x.ArchiveUnitId == model.ArchiveUnitId);
            if (data != null)
            {
                model.IsActive = data.IsActive;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                _context.TrxArchiveUnits.Update(model);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }
}
