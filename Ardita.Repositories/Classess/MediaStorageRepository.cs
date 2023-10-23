using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace Ardita.Repositories.Classess;

public class MediaStorageRepository : IMediaStorageRepository
{
    private readonly BksArditaDevContext _context;
    private readonly ILogChangesRepository _logChangesRepository;
    public MediaStorageRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
    {
        _context = context;
        _logChangesRepository = logChangesRepository;
    }
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

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxMediaStorage>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<TrxMediaStorage> { data }, new List<TrxMediaStorage> {  });
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }

    public async Task<int> GetCountByRackList(bool isActive) => 
        await _context.TrxRacks
            .Include(x => x.TrxLevels).ThenInclude(x => x.TrxRows).ThenInclude(x => x.TrxMediaStorages)
            .Where(x => x.TrxLevels.Any(y => y.TrxRows.Any(z => z.TrxMediaStorages.Any())))
        .Where(x => isActive ? x.Room.ArchiveRoomType == GlobalConst.UnitPengolah : x.Room.ArchiveRoomType == GlobalConst.UnitKearsipan)
            .AsNoTracking()
            .CountAsync();
    public async Task<IEnumerable<TrxMediaStorage>> GetAll(string par = " 1=1 ") => 
        await _context.TrxMediaStorages
            .Include(g => g.GmdDetail)
            .Include(d => d.TrxMediaStorageDetails)
                .ThenInclude(a => a.Archive)
                .ThenInclude(c => c.Creator)
            .Include(s => s.SubjectClassification.Classification)
            .Include(t => t.TypeStorage.ArchiveUnit)
            .Include(t => t.TypeStorage.TrxTypeStorageDetails)
            .Include(r => r.Row!.Level!.Rack!.Room!.Floor)
                .Where(par)
        .AsNoTracking()
            .ToListAsync();
    public async Task<int> GetCount(string par = " 1=1 ") => 
        await _context.TrxMediaStorages
            .Include(g => g.GmdDetail)
            .Include(d => d.TrxMediaStorageDetails)
                .ThenInclude(a => a.Archive)
                .ThenInclude(c => c.Creator)
            .Include(s => s.SubjectClassification.Classification)
            .Include(t => t.TypeStorage.ArchiveUnit)
            .Include(t => t.TypeStorage.TrxTypeStorageDetails)
            .Include(r => r.Row!.Level!.Rack!.Room!.Floor)
                .Where(par)
        .AsNoTracking()
            .CountAsync();

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        var User = AppUsers.CurrentUser(model.SessionUser);
        if (model.sortColumn == "row.level.rack.rackName")
        {
            model.sortColumn = $"row.level.rack.rackName {model.sortColumnDirection}, row.level.levelName {model.sortColumnDirection}, row.rowName {model.sortColumnDirection}";
            model.sortColumnDirection = string.Empty;
        }

        var result = await _context.TrxMediaStorages
            .Include(x => x.Status)
            .Include(x => x.SubjectClassification.Classification.Creator)
            .Include(x => x.TypeStorage.ArchiveUnit)
            .Include(x => x.Row.Level.Rack.Room.Floor)
            .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.Archive)
            .Where(x => (x.MediaStorageCode + x.SubjectClassification.SubjectClassificationName + x.Status.Name 
            + x.SubjectClassification.Classification.Creator.CreatorName + x.TypeStorage.TypeStorageName + x.TypeStorage.ArchiveUnit.ArchiveUnitName).Contains(model.searchValue!))
            .Where(x => x.IsActive == true)
            .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.SubjectClassification.Classification.Creator.ArchiveUnitId == User.ArchiveUnitId))
            .Where(x => (User.CreatorId == Guid.Empty ? true : x.SubjectClassification.Classification.CreatorId == User.CreatorId))
            .Where(model.advanceSearch!.Search)
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
            .Skip(model.skip).Take(model.pageSize)
            .Select(x => new
            {
                x.MediaStorageId,
                x.MediaStorageCode,
                LabelCode = x.Row.Level!.Rack!.RackName + "-" + x.Row.Level.LevelName + "-" + x.Row.RowName,
                x.StatusId,
                Status = x.Status.Name == "Submit" ? "Tersimpan" : x.Status.Name,
                x.Status.Color,
                x.ArchiveYear,
                x.TypeStorage.ArchiveUnit.ArchiveUnitName,
                x.TypeStorage.TypeStorageName,
                x.SubjectClassification.SubjectClassificationName,
                x.SubjectClassification.Classification.Creator.CreatorName
            })
            .ToListAsync();

        return result;
    }
    public async Task<int> GetCountByFilterModel(DataTableModel model)
    {
        var User = AppUsers.CurrentUser(model.SessionUser);
        var result = await _context.TrxMediaStorages
            .Include(x => x.Status)
            .Include(x => x.SubjectClassification.Classification.Creator)
            .Include(x => x.TypeStorage.ArchiveUnit)
            .Include(x => x.Row.Level.Rack.Room.Floor)
            .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.Archive)
            .Where(x => (x.MediaStorageCode + x.SubjectClassification.SubjectClassificationName + x.Status.Name
            + x.SubjectClassification.Classification.Creator.CreatorName + x.TypeStorage.TypeStorageName + x.TypeStorage.ArchiveUnit.ArchiveUnitName).Contains(model.searchValue!))
            .Where(x => x.IsActive == true)
            .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.SubjectClassification.Classification.Creator.ArchiveUnitId == User.ArchiveUnitId))
            .Where(x => (User.CreatorId == Guid.Empty ? true : x.SubjectClassification.Classification.CreatorId == User.CreatorId))
            .Where(model.advanceSearch!.Search)
            .CountAsync();

        return result;
    }
    public async Task<TrxMediaStorage> GetById(Guid id) 
    {

        var total = await _context.TrxMediaStorageDetails
            .Include(d => d.Archive)
            .Where(x => x.MediaStorageId == id)
            .Where(x => x.IsActive == true)
            .Where(x => x.Archive.IsActive == true)
            .SumAsync(x => x.Archive.Volume);

        var data = await _context.TrxMediaStorages
            .Include(g => g.GmdDetail)
            .Include(d => d.TrxMediaStorageDetails.Where(w => w.IsActive))
                .ThenInclude(a => a.Archive)
                .ThenInclude(c => c.Creator)
            .Include(s => s.SubjectClassification)
            .Include(t => t.TypeStorage).ThenInclude(a => a.ArchiveUnit)
            .Include(r => r.Row!.Level!.Rack!.Room!.Floor).AsNoTracking()
            .Where(x => x.MediaStorageId == id)
            .FirstAsync();

        data.DifferenceVolume = data.TotalVolume - total; 

        _context.Entry(data).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return data;
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
            var subjectClassification = await _context.TrxSubjectClassifications.FirstOrDefaultAsync(x => x.SubjectClassificationId == model.SubjectClassificationId);
            var classificationData = await _context.TrxClassifications.FirstOrDefaultAsync(x => x.ClassificationId == subjectClassification!.ClassificationId);
            var creatorData = await _context.MstCreators.FirstOrDefaultAsync(x => x.CreatorId == classificationData!.CreatorId);
            var typeStorageData = await _context.TrxTypeStorages.FirstOrDefaultAsync(x => x.TypeStorageId == model.TypeStorageId);
            var storageCode = $"{creatorData!.CreatorCode}.{typeStorageData!.TypeStorageCode}.{model.ArchiveYear}";

            var countData = await _context.TrxMediaStorages.Where(x => x.MediaStorageCode.Contains(storageCode)).CountAsync();
            int i = 1;
            var validStorageCode = string.Empty;
            bool inValid = true;
            while (inValid)
            {
                validStorageCode = $"{storageCode}.{(countData + i).ToString("D3")}";
                int count = await _context.TrxMediaStorages.Where(x => x.MediaStorageCode == validStorageCode).CountAsync();
                if(count > 0)
                    i++;
                else
                    inValid = false;
            }

            model.MediaStorageCode = validStorageCode;

            foreach (var e in _context.ChangeTracker.Entries())
            {
                e.State = EntityState.Detached;
            }

            model.IsActive = true;

            _context.Entry(model).State = EntityState.Added;
            await _context.SaveChangesAsync();

            //Insert Detail
            if (detail.Any())
            {
                foreach (var item in detail)
                {
                    item.MediaStorageId = model.MediaStorageId;
                    item.IsActive = true;
                    _context.TrxMediaStorageDetails.Add(item);
                    result += await _context.SaveChangesAsync();
                }
            }

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<TrxMediaStorage>(GlobalConst.New, model.CreatedBy, new List<TrxMediaStorage> {  }, new List<TrxMediaStorage> { model });
                    await _logChangesRepository.CreateLog<TrxMediaStorageDetail>(GlobalConst.New, model.CreatedBy, new List<TrxMediaStorageDetail> {  }, detail);
                }
                catch (Exception ex) { }
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

                foreach (var e in _context.ChangeTracker.Entries())
                {
                    e.State = EntityState.Detached;
                }

                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                //Update Detail
                var dataDetail = await _context.TrxMediaStorageDetails.AsNoTracking().Where(x => x.MediaStorageId == model.MediaStorageId).ToListAsync();
                if (dataDetail.Any())
                {
                    _context.TrxMediaStorageDetails.RemoveRange(dataDetail);
                    result += await _context.SaveChangesAsync();
                }

                if (detail.Any())
                {
                    foreach (var item in detail)
                    {
                        item.CreatedDate = (DateTime)model.UpdatedDate!;
                        item.CreatedBy = (Guid)model.UpdatedBy!;
                        item.MediaStorageId = model.MediaStorageId;
                        item.IsActive = true;
                        _context.TrxMediaStorageDetails.Add(item);
                        result += await _context.SaveChangesAsync();
                    }
                }

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxMediaStorage>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<TrxMediaStorage> { data }, new List<TrxMediaStorage> { model });
                        await _logChangesRepository.CreateLog<TrxMediaStorageDetail>(GlobalConst.Update, (Guid)model.UpdatedBy!, dataDetail, detail);
                    }
                    catch (Exception ex) { }
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

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxMediaStorageDetail>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<TrxMediaStorageDetail> { data }, new List<TrxMediaStorageDetail> { model });
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }

    public async Task<bool> UpdateDetailIsUsed(Guid archiveId, string usedBy, bool isUsed)
    {
        await _context.Database.ExecuteSqlAsync($"UPDATE TRX_ARCHIVE SET is_used = {(isUsed ? 1 : 0)}, is_used_date = {DateTime.Now}, is_used_by = '{usedBy}' WHERE archive_id = {archiveId}");
        await _context.SaveChangesAsync();
        return true;
    }
}
