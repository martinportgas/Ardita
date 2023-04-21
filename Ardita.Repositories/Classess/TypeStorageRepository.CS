using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class TypeStorageRepository : ITypeStorageRepository
{
    private readonly BksArditaDevContext _context;

    public async Task<int> Delete(TrxTypeStorage model)
    {
        int result = 0;

        if (model.TypeStorageId != Guid.Empty)
        {
            _context.TrxTypeStorages.Remove(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<IEnumerable<TrxTypeStorage>> GetAll() => await _context.TrxTypeStorages.Where(x => x.IsActive == true).ToListAsync();
    

    public async Task<IEnumerable<TrxTypeStorage>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<TrxTypeStorage> result;

        var propertyInfo = typeof(TrxTypeStorage).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(TrxTypeStorage).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.TrxTypeStorages
            .Where(x => (x.TypeStorageCode + x.TypeStorageName).Contains(model.searchValue))
            .OrderBy(x => EF.Property<TrxTypeStorage>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }
        else
        {
            result = await _context.TrxTypeStorages
            .Where(x => (x.TypeStorageCode + x.TypeStorageName).Contains(model.searchValue))
            .OrderByDescending(x => EF.Property<TrxTypeStorage>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }

        return result;
    }

    public async Task<IEnumerable<TrxTypeStorage>> GetById(Guid id) => await _context.TrxTypeStorages.Where(x => x.TypeStorageId == id).ToListAsync();

    public async Task<int> GetCount() => await _context.TrxTypeStorages.CountAsync();

    public async Task<int> Insert(TrxTypeStorage model)
    {
        int result = 0;

        if (model != null)
        {
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
            var data = await _context.TrxTypeStorages.AsNoTracking().Where(x => x.TypeStorageId == model.TypeStorageId).ToListAsync();
            if (data != null)
            {
                _context.Update(model);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }
}
