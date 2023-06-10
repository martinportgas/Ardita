using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class GmdRepository : IGmdRepository
{
    private readonly BksArditaDevContext _context;

    public GmdRepository(BksArditaDevContext context) => _context = context;

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
            }
        }
        return result;
    }

    public async Task<IEnumerable<MstGmd>> GetAll() => await _context.MstGmds.Where(x => x.IsActive == true).AsNoTracking().ToListAsync();
    
    public async Task<IEnumerable<MstGmd>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<MstGmd> result;

        var propertyInfo = typeof(MstGmd).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(MstGmd).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.MstGmds
                .Where(x => (x.GmdCode + x.GmdName)
                .Contains(model.searchValue) && x.IsActive)
                .OrderBy(x => EF.Property<MstGmd>(x, propertyName)).ThenBy(x => x.CreatedDate).ThenBy(x => x.UpdatedDate)
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }
        else
        {
            result = await _context.MstGmds
                .Where(x => (x.GmdCode + x.GmdName)
                .Contains(model.searchValue) && x.IsActive)
                .OrderByDescending(x => EF.Property<MstGmd>(x, propertyName)).ThenBy(x => x.CreatedDate).ThenBy(x => x.UpdatedDate)
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }

        return result;
    }

    public async Task<IEnumerable<MstGmd>> GetById(Guid id) => await _context.MstGmds.Where(x => x.GmdId== id).ToListAsync();

    public async Task<int> GetCount() => await _context.MstGmds.CountAsync(x => x.IsActive == true);

    public async Task<int> Insert(MstGmd model)
    {
        int result = 0;

        if (model != null)
        {
            model.IsActive = true;
            _context.MstGmds.Add(model);
            result = await _context.SaveChangesAsync();
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
        }
        return result;
    }

    public async Task<int> Update(MstGmd model)
    {
        int result = 0;

        if (model != null && model.GmdId != Guid.Empty)
        {
            var data = await _context.MstGmds.AsNoTracking().FirstAsync(x => x.GmdId == model.GmdId);
            if (data != null)
            {
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                model.IsActive = data.IsActive;
                _context.Update(model);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }
}
