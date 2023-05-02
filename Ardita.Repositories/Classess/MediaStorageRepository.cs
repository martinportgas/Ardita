using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class MediaStorageRepository : IMediaStorageRepository
{
    private readonly BksArditaDevContext _context;

    public MediaStorageRepository(BksArditaDevContext context) => _context = context;
    public async Task<int> Delete(TrxMediaStorage model)
    {
        int result = 0;

        if (model != null && model.MediaStorageId != Guid.Empty)
        {
            var data = await _context.TrxMediaStorages.AsNoTracking().FirstAsync(x => x.MediaStorageId == model.MediaStorageId);
            if (data != null)
            {
                data.IsActive = false;
                data.UpdatedBy = model.UpdatedBy;
                data.UpdatedDate = model.UpdatedDate;
                _context.TrxMediaStorages.Update(data);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }

    public async Task<IEnumerable<TrxMediaStorage>> GetAll() => await _context.TrxMediaStorages.AsNoTracking().Where(x => x.IsActive == true).ToListAsync();

    public async Task<IEnumerable<TrxMediaStorage>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<TrxMediaStorage> result;

        var propertyInfo = typeof(TrxMediaStorage).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(TrxMediaStorage).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.TrxMediaStorages
            .Where(x => (x.MediaStorageCode + x.MediaStorageName).Contains(model.searchValue) && x.IsActive == true)
            .OrderBy(x => EF.Property<TrxMediaStorage>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }
        else
        {
            result = await _context.TrxMediaStorages
            .Where(x => (x.MediaStorageCode + x.MediaStorageName).Contains(model.searchValue) && x.IsActive == true)
            .OrderByDescending(x => EF.Property<TrxMediaStorage>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }

        return result;
    }

    public async Task<TrxMediaStorage> GetById(Guid id) 
    {
        return await _context.TrxMediaStorages
            .Include(d => d.TrxMediaStorageDetails.Where(w => w.IsActive))
                .ThenInclude(a => a.Archive)
                .ThenInclude(s => s.SubSubjectClassification)
                .ThenInclude(c => c.Creator)
            .Include(t => t.TypeStorage).ThenInclude(a => a.ArchiveUnit)
            .Include(r => r.Row.Level.Rack.Room.Floor).AsNoTracking()
            .Where(x => x.MediaStorageId == id)
            .FirstAsync();
    }
    public async Task<TrxMediaStorageDetail> GetDetailByArchiveId(Guid id)
    {
        return await _context.TrxMediaStorageDetails.FirstOrDefaultAsync(x => x.ArchiveId == id);
    }

    public async Task<int> GetCount() => await _context.TrxMediaStorages.CountAsync(x => x.IsActive == true);

    public async Task<int> Insert(TrxMediaStorage model, List<TrxMediaStorageDetail> detail)
    {
        int result = 0;

        if (model != null)
        {
            //Insert Header
            model.IsActive = true;
            var Row = _context.TrxRows.FirstOrDefault(r => r.RowId == model.RowId);
            var TypeStorage = _context.TrxTypeStorages.FirstOrDefault(r => r.TypeStorageId == model.TypeStorageId);

            foreach (var e in _context.ChangeTracker.Entries())
            {
                e.State = EntityState.Detached;
            }

            model.RowId = Row.RowId;
            model.TypeStorageId = TypeStorage.TypeStorageId;

            _context.Entry(model).State = EntityState.Added;
            await _context.SaveChangesAsync();

            //Insert Detail
            foreach (var item in detail)
            {
                item.MediaStorageId = model.MediaStorageId;
                item.IsActive = true;
                _context.TrxMediaStorageDetails.Add(item);
                result += await _context.SaveChangesAsync();
            }
        }
        return result;
    }

    public async Task<int> Update(TrxMediaStorage model, List<TrxMediaStorageDetail> detail)
    {
        int result = 0;

        if (model != null && model.MediaStorageId != Guid.Empty)
        {
            var data = await _context.TrxMediaStorages.AsNoTracking().FirstAsync(x => x.MediaStorageId == model.MediaStorageId);
            if (data != null)
            {
                //Update Header
                model.IsActive = data.IsActive;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;

                var Row = _context.TrxRows.FirstOrDefault(r => r.RowId == model.RowId);
                var TypeStorage = _context.TrxTypeStorages.FirstOrDefault(r => r.TypeStorageId == model.TypeStorageId);

                foreach (var e in _context.ChangeTracker.Entries())
                {
                    e.State = EntityState.Detached;
                }

                model.RowId = Row.RowId;
                model.TypeStorageId = TypeStorage.TypeStorageId;
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                //Update Detail
                var dataDetail = await _context.TrxMediaStorageDetails.AsNoTracking().Where(x => x.MediaStorageId == model.MediaStorageId).ToListAsync();

                _context.TrxMediaStorageDetails.RemoveRange(dataDetail);
                result += await _context.SaveChangesAsync();

                foreach (var item in detail)
                {
                    item.CreatedDate = (DateTime)model.UpdatedDate;
                    item.CreatedBy = (Guid)model.UpdatedBy;
                    item.MediaStorageId = model.MediaStorageId;
                    _context.TrxMediaStorageDetails.Add(item);
                    result += await _context.SaveChangesAsync();
                }
            }
        }
        return result;
    }
    public async Task<int> UpdateDetail(TrxMediaStorageDetail model)
    {
        int result = 0;

        if (model != null && model.MediaStorageDetailId != Guid.Empty)
        {
            var data = await _context.TrxMediaStorageDetails.AsNoTracking().FirstAsync(x => x.MediaStorageDetailId == model.MediaStorageDetailId);
            if (data != null)
            {
                _context.Update(model);

                var countByHeader = await _context.TrxMediaStorageDetails.CountAsync(x => x.MediaStorageId == model.MediaStorageId && x.IsActive == true);
                if(countByHeader == 0)
                {
                    var header = await _context.TrxMediaStorages.FirstOrDefaultAsync(x => x.MediaStorageId == model.MediaStorageId);
                    if(header != null)
                    {
                        header.IsActive = false;
                        _context.Update(header);
                    }
                }

                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }
}
