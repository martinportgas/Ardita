using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess;

public class SubTypeStorageRepository : ISubTypeStorageRepository
{
    private readonly BksArditaDevContext _context;

    public SubTypeStorageRepository(BksArditaDevContext context)
    {
        _context = context;
    }

    public async Task<int> Delete(MstSubTypeStorage model)
    {
        int result = 0;

        var data = await _context.MstSubTypeStorages
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SubTypeStorageId == model.SubTypeStorageId);

        if (data != null)
        {
            data.IsActive = false;
            data.UpdatedDate = model.UpdatedDate;
            data.UpdatedBy = model.UpdatedBy;
            _context.MstSubTypeStorages.Update(data);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<int> DeleteIDXSubTypeStorage(Guid id)
    {
         int result = 0;

        var data = await _context.IdxSubTypeStorages
            .AsNoTracking()
            .Where(x => x.SubTypeStorageId == id).ToListAsync();

        if (data != null)
        {
            _context.IdxSubTypeStorages.RemoveRange(data);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<IEnumerable<MstSubTypeStorage>> GetAll()
    {
        return await _context.MstSubTypeStorages
            .Include(x => x.IdxSubTypeStorages)
            .ThenInclude(x => x.TypeStorage)
            .AsNoTracking()
            .Where(x => x.IsActive == true)
            .ToListAsync();
    }

    public async Task<IEnumerable<MstSubTypeStorage>> GetAllByTypeStorageId(Guid ID)
    {
        var result = from idx in _context.IdxSubTypeStorages
                     join mst in _context.MstSubTypeStorages on idx.SubTypeStorageId equals mst.SubTypeStorageId
                     join trx in _context.TrxTypeStorages on idx.TypeStorageId equals trx.TypeStorageId
                     where idx.TypeStorageId == ID
                     select mst;

        await Task.Delay(0);

        return result;
    }

    public async Task<IEnumerable<IdxSubTypeStorage>> GetAllBySubTypeStorageId(Guid ID)
    {
        var result = await _context.IdxSubTypeStorages.AsNoTracking()
            .Include(x => x.TypeStorage)
            .ThenInclude(x => x.ArchiveUnit)
            .Where(x => x.SubTypeStorageId == ID).ToListAsync();
       
        return result;
    }

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        var result = await _context.MstSubTypeStorages
              .Include(x => x.IdxSubTypeStorages)
              .ThenInclude(x => x.TypeStorage)
              .Where(x => x.IsActive == true)
              .Where($"(SubTypeStorageCode+SubTypeStorageName+IdxSubTypeStorages.FirstOrDefault().TypeStorage.TypeStorageCode+IdxSubTypeStorages.FirstOrDefault().TypeStorage.TypeStorageName).Contains(@0)", model.searchValue)
              .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
              .Skip(model.skip).Take(model.pageSize)
              .Select(x => new
              {
                  x.SubTypeStorageId,
                  x.SubTypeStorageCode,
                  x.SubTypeStorageName,
                  TypeStorageCode = x.IdxSubTypeStorages.FirstOrDefault().TypeStorage.TypeStorageCode,
                  TypeStorageName = x.IdxSubTypeStorages.FirstOrDefault().TypeStorage.TypeStorageName
              })
              .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<MstSubTypeStorage>> GetById(Guid id)
    {
        var data = await _context.MstSubTypeStorages
            .Include(x => x.IdxSubTypeStorages)
            .ThenInclude(x => x.TypeStorage)
           .AsNoTracking()
           .Where(x => x.SubTypeStorageId == id)
           .ToListAsync();
        return data;
    }

    public async Task<int> GetCount()
    {
        return await _context.MstSubTypeStorages.AsNoTracking()
             .Include(x => x.IdxSubTypeStorages).ThenInclude(x => x.TypeStorage)
             .Where(x => x.IsActive == true)
             .CountAsync();
    }

    public async Task<int> GetCountByFilterModel(DataTableModel model)
    {
        var result = await _context.MstSubTypeStorages
              .Include(x => x.IdxSubTypeStorages)
              .ThenInclude(x => x.TypeStorage)
              .Where(x => x.IsActive == true)
              .Where($"(SubTypeStorageCode+SubTypeStorageName+IdxSubTypeStorages.FirstOrDefault().TypeStorage.TypeStorageCode+IdxSubTypeStorages.FirstOrDefault().TypeStorage.TypeStorageName).Contains(@0)", model.searchValue)
              .CountAsync();
        return result;
    }

    public async Task<int> Insert(MstSubTypeStorage model)
    {
        int result = 0;

        if (model != null)
        {
            var data = await _context.MstSubTypeStorages.AsNoTracking()
                .CountAsync(x=> x.SubTypeStorageCode == model.SubTypeStorageCode && x.SubTypeStorageName == model.SubTypeStorageName);
            if (data == 0)
            {
                model.IsActive = true;
                _context.MstSubTypeStorages.Add(model);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }

    public async Task<bool> InsertBulk(List<MstSubTypeStorage> mstSubTypeStorages)
    {
        bool result = false;
        if (mstSubTypeStorages.Count() > 0)
        {
            await _context.AddRangeAsync(mstSubTypeStorages);
            await _context.SaveChangesAsync();
            result = true;
        }
        return result;
    }

    public async Task<bool> InsertBulkIDXTypeStorage(List<IdxSubTypeStorage> idxSubTypeStorages)
    {
        bool result = false;
        if (idxSubTypeStorages.Count() > 0)
        {
            await _context.AddRangeAsync(idxSubTypeStorages);
            await _context.SaveChangesAsync();
            result = true;
        }
        return result;
    }

    public async Task<int> InsertIDXSubTypeStorage(IdxSubTypeStorage model)
    {
        int result = 0;

        if (model != null)
        {
            _context.IdxSubTypeStorages.Add(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<int> Update(MstSubTypeStorage model)
    {
        int result = 0;

        if (model != null && model.SubTypeStorageId != Guid.Empty)
        {
            var data = await _context.MstSubTypeStorages.AsNoTracking().FirstAsync(x => x.SubTypeStorageId == model.SubTypeStorageId);
            if (data != null)
            {
                model.IsActive = data.IsActive;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                _context.MstSubTypeStorages.Update(model);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }

}
