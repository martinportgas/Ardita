using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class ArchiveCreatorRepository : IArchiveCreatorRepository
{
    private readonly BksArditaDevContext _context;

    public async Task<int> Delete(MstCreator model)
    {
        int result = 0;

        if (model.CreatorId != Guid.Empty)
        {
            _context.MstCreators.Remove(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<IEnumerable<MstCreator>> GetAll() => await _context.MstCreators.Where(x => x.IsActive == true).ToListAsync();

    public async Task<IEnumerable<MstCreator>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<MstCreator> result;

        var propertyInfo = typeof(MstCreator).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(MstCreator).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.MstCreators
            .Where(x => (x.CreatorCode + x.CreatorName).Contains(model.searchValue))
            .OrderBy(x => EF.Property<MstCreator>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }
        else
        {
            result = await _context.MstCreators
            .Where(x => (x.CreatorCode + x.CreatorName).Contains(model.searchValue))
            .OrderByDescending(x => EF.Property<MstCreator>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }

        return result;
    }

    public async Task<IEnumerable<MstCreator>> GetById(Guid id) => await _context.MstCreators.Where(x => x.CreatorId== id).ToListAsync();

    public async Task<int> GetCount() => await _context.MstCreators.CountAsync();

    public async Task<int> Insert(MstCreator model)
    {
        int result = 0;

        if (model != null)
        {
            _context.MstCreators.Add(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<int> Update(MstCreator model)
    {
        int result = 0;

        if (model != null && model.CreatorId != Guid.Empty)
        {
            var data = await _context.MstCreators.AsNoTracking().Where(x => x.CreatorId == model.CreatorId).ToListAsync();
            if (data != null)
            {
                _context.Update(model);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }
}
