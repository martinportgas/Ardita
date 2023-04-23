using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class TypeStorageRepository : ITypeStorageRepository
{
    private readonly BksArditaDevContext _context;
    
    public TypeStorageRepository(BksArditaDevContext context) => _context = context;
    
    public async Task<IEnumerable<TrxTypeStorage>> GetAll() => await _context.TrxTypeStorages.Where(x => x.IsActive == true).ToListAsync();
    
    public async Task<IEnumerable<TrxTypeStorage>> GetById(Guid id) => await _context.TrxTypeStorages.Where(x => x.TypeStorageId == id).ToListAsync();
    
    public async Task<int> GetCount() => await _context.TrxTypeStorages.CountAsync(x => x.IsActive == true);

    public async Task<int> Delete(TrxTypeStorage model)
    {
        int result = 0;

        if (model.TypeStorageId != Guid.Empty)
        {
            var data = await _context.TrxTypeStorages.AsNoTracking().FirstAsync(x => x.TypeStorageId == model.TypeStorageId);
            if (data != null)
            {
                data.IsActive = false;
                data.UpdatedDate = model.UpdatedDate;
                data.UpdatedBy = model.UpdatedBy;
                _context.TrxTypeStorages.Update(data);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }

    public async Task<IEnumerable<TrxTypeStorage>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<TrxTypeStorage> result;

        var propertyInfo = typeof(TrxTypeStorage).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(TrxTypeStorage).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.TrxTypeStorages
            .Where(x => (x.TypeStorageCode + x.TypeStorageName).Contains(model.searchValue) && x.IsActive == true)
            .OrderBy(x => EF.Property<TrxTypeStorage>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }
        else
        {
            result = await _context.TrxTypeStorages
            .Where(x => (x.TypeStorageCode + x.TypeStorageName).Contains(model.searchValue) && x.IsActive == true)
            .OrderByDescending(x => EF.Property<TrxTypeStorage>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }

        return result;
    }

    public async Task<int> Insert(TrxTypeStorage model)
    {
        int result = 0;

        if (model != null)
        {
            model.IsActive = true;
            _context.TrxTypeStorages.Add(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<int> Update(TrxTypeStorage model)
    {
        int result = 0;

        if (model != null && model.TypeStorageId != Guid.Empty)
        {
            var data = await _context.TrxTypeStorages.AsNoTracking().FirstAsync(x => x.TypeStorageId == model.TypeStorageId);
            if (data != null)
            {
                model.IsActive = data.IsActive;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                _context.TrxTypeStorages.Update(model);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }
}
