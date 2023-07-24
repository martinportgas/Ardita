using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class ArchiveCreatorRepository : IArchiveCreatorRepository
{
    private readonly BksArditaDevContext _context;

    public ArchiveCreatorRepository(BksArditaDevContext context) => _context = context;

    public async Task<int> Delete(MstCreator model)
    {
        int result = 0;

        if (model.CreatorId != Guid.Empty)
        {
            var data = await _context.MstCreators.AsNoTracking().FirstAsync(x => x.CreatorId == model.CreatorId);
            if (data != null)
            {
                data.IsActive = false;
                data.UpdatedDate = model.UpdatedDate;
                data.UpdatedBy = model.UpdatedBy;
                _context.MstCreators.Update(data);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }

    public async Task<IEnumerable<MstCreator>> GetAll() 
        => await _context.MstCreators
        .Include(x => x.TrxArchives)
        .Include(x=> x.ArchiveUnit)
        .ThenInclude(x=> x.Company)
        .Where(x => x.IsActive == true)
        .ToListAsync();

    public async Task<IEnumerable<MstCreator>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<MstCreator> result;

        var propertyInfo = typeof(MstCreator).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(MstCreator).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.MstCreators
            .Where(x => (x.CreatorCode + x.CreatorName).Contains(model.searchValue) && x.IsActive == true)
            .OrderBy(x => EF.Property<MstCreator>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }
        else
        {
            result = await _context.MstCreators
            .Where(x => (x.CreatorCode + x.CreatorName).Contains(model.searchValue) && x.IsActive == true)
            .OrderByDescending(x => EF.Property<MstCreator>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }

        return result;
    }

    public async Task<IEnumerable<MstCreator>> GetById(Guid id) => await _context.MstCreators.Where(x => x.CreatorId== id).Include(x => x.ArchiveUnit).AsNoTracking().ToListAsync();

    public async Task<int> GetCount() => await _context.MstCreators.CountAsync(x => x.IsActive == true);

    public async Task<int> Insert(MstCreator model)
    {
        int result = 0;

        if (model != null)
        {
            model.IsActive = true;
            _context.MstCreators.Add(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<bool> InsertBulk(List<MstCreator> mstCreators)
    {
        bool result = false;
        if (mstCreators.Count() > 0)
        {
            await _context.AddRangeAsync(mstCreators);
            await _context.SaveChangesAsync();
            result = true;
        }
        return result;
    }

    public async Task<int> Update(MstCreator model)
    {
        int result = 0;

        if (model != null && model.CreatorId != Guid.Empty)
        {
            var data = await _context.MstCreators.AsNoTracking().FirstAsync(x => x.CreatorId == model.CreatorId);
            if (data != null)
            {
                model.IsActive = data.IsActive;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                _context.MstCreators.Update(model);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }
}
