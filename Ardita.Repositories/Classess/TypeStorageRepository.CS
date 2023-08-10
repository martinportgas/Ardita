using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Dynamic.Core;
using Ardita.Models.LogModels;
using System.Text.Json;
using Ardita.Extensions;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;

namespace Ardita.Repositories.Classess;

public class TypeStorageRepository : ITypeStorageRepository
{
    private readonly BksArditaDevContext _context;
    private readonly ILogChangesRepository _logChangesRepository;
    
    public TypeStorageRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
    {
        _context = context;
        _logChangesRepository = logChangesRepository;
    }
    
    public async Task<IEnumerable<TrxTypeStorage>> GetAll() 
        => await _context.TrxTypeStorages
        .Include(x => x.ArchiveUnit.Company)
        .Where(x => x.IsActive == true)
        .Where(x => x.ArchiveUnit.IsActive == true)
        .Where(x => x.ArchiveUnit.Company.IsActive == true)
        .ToListAsync();
    
    public async Task<TrxTypeStorage> GetById(Guid id) 
        => await _context.TrxTypeStorages
        .Include(x => x.TrxTypeStorageDetails)
        .ThenInclude(x =>x.GmdDetail)
        .AsNoTracking()
        .FirstAsync(x => x.TypeStorageId == id);

    public async Task<IEnumerable<TrxTypeStorageDetail>> GetAllByTypeStorageId(Guid TypeStorageId)
        => await _context.TrxTypeStorageDetails
        .Include(x => x.TypeStorage)
        .Include(x => x.GmdDetail.Gmd)

        .Where(x => x.TypeStorageId == TypeStorageId)
        .ToListAsync();

    
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

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxTypeStorage>(GlobalConst.Delete, model.CreatedBy, new List<TrxTypeStorage> { data }, new List<TrxTypeStorage> {  });
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        var result = await _context.TrxTypeStorages
            .Include(x => x.ArchiveUnit)
            .Where(x => (x.TypeStorageCode + x.TypeStorageName).Contains(model.searchValue) && x.IsActive == true)
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
            .Skip(model.skip).Take(model.pageSize)
            .Select(x => new {
                x.TypeStorageId,
                x.TypeStorageCode,
                x.TypeStorageName,
                x.ArchiveUnit.ArchiveUnitName,
                x.ArchiveUnit.Company.CompanyName
            })
            .ToListAsync();

        return result;
    }
    public async Task<int> GetCountByFilterModel(DataTableModel model)
    {
        var result = await _context.TrxTypeStorages
            .Include(x => x.ArchiveUnit)
            .Where(x => (x.TypeStorageCode + x.TypeStorageName).Contains(model.searchValue) && x.IsActive == true)
            .CountAsync();

        return result;
    }

    public async Task<int> Insert(TrxTypeStorage model, List<TrxTypeStorageDetail> detail)
    {
        int result = 0;
        List<TrxTypeStorageDetail> details = new();


        if (model != null)
        {
            model.TypeStorageId = Guid.NewGuid();
            model.IsActive = true;
            _context.TrxTypeStorages.Add(model);
            result = await _context.SaveChangesAsync();

            foreach (var item in detail)
            {
                item.CreatedDate = model.CreatedDate;
                item.CreatedBy = model.CreatedBy;
                item.TypeStorageId = model.TypeStorageId;

                details.Add(item);
            }

            _context.TrxTypeStorageDetails.AddRange(details);
            result += await _context.SaveChangesAsync();

            //Log
            if (result > 0) 
            {
                try 
                {
                    await _logChangesRepository.CreateLog<TrxTypeStorage>(GlobalConst.New, model.CreatedBy, new List<TrxTypeStorage> { }, new List<TrxTypeStorage> { model });
                    await _logChangesRepository.CreateLog<TrxTypeStorageDetail>(GlobalConst.New, model.CreatedBy, new List<TrxTypeStorageDetail> { }, details);

                    //var objLog = new LogRequestModel();
                    //objLog.TableName = $"{nameof(TrxTypeStorage)}, {nameof(TrxTypeStorageDetail)}";

                    //var header = await _context.TrxTypeStorages.Include(x => x.ArchiveUnit).AsNoTracking().Where(x => x.TypeStorageId == model.TypeStorageId).ToListAsync();
                    //var child = await _context.TrxTypeStorageDetails.Include(x => x.TypeStorage).AsNoTracking().Where(x => x.TypeStorageId == model.TypeStorageId).ToListAsync();
                    //var users = await _context.MstUsers.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == model.CreatedBy);

                    //var objDetail = new LogRequestDetailModel();
                    //objDetail.Header = header.ToList();
                    //objDetail.Child = child.ToList();
                    //objLog.data = objDetail;

                    //var objLogDB = new LogChange();
                    //objLogDB.UserId = users.UserId;
                    //objLogDB.Username = users.Username;
                    //objLogDB.TableReference = $"{nameof(TrxTypeStorage)}, {nameof(TrxTypeStorageDetail)}";
                    //objLogDB.ChangeDate = DateTime.Now;
                    //objLogDB.NewValue = JsonConvert.SerializeObject(objDetail, Formatting.Indented, new JsonSerializerSettings()
                    //{
                    //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    //});

                    //_context.LogChanges.Add(objLogDB);
                    //await _context.SaveChangesAsync();
                } catch (Exception ex) { }
            }
        }
        return result;
    }
    public async Task<bool> InsertBulk(List<TrxTypeStorage> rows)
    {
        bool result = false;
        if (rows.Count() > 0)
        {
            await _context.AddRangeAsync(rows);
            await _context.SaveChangesAsync();
            result = true;
        }
        //Log
        if (result)
        {
            try
            {
                await _logChangesRepository.CreateLog<TrxTypeStorage>(GlobalConst.New, rows.FirstOrDefault().CreatedBy, new List<TrxTypeStorage> { }, rows);
            }
            catch (Exception ex) { }
        }
        return result;
    }

    public async Task<int> Update(TrxTypeStorage model, List<TrxTypeStorageDetail> detail)
    {
        int result = 0;
        List<TrxTypeStorageDetail> newDetail = new();
        List<TrxTypeStorageDetail> oldDetail = new();

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

                //Insert Detail
                oldDetail = await _context.TrxTypeStorageDetails.AsNoTracking().Where(x => x.TypeStorageId == model.TypeStorageId).ToListAsync();
                if (oldDetail.Any())
                {
                    _context.TrxTypeStorageDetails.RemoveRange(oldDetail);
                    result += await _context.SaveChangesAsync();
                }

                foreach (var item in detail)
                {
                    item.CreatedDate = model.CreatedDate;
                    item.CreatedBy = model.CreatedBy;
                    item.TypeStorageId = model.TypeStorageId;

                    newDetail.Add(item);
                }

                _context.TrxTypeStorageDetails.AddRange(newDetail);
                result += await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxTypeStorage>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<TrxTypeStorage> { data }, new List<TrxTypeStorage> { model });
                        await _logChangesRepository.CreateLog<TrxTypeStorageDetail>(GlobalConst.Update, (Guid)model.UpdatedBy!, oldDetail, newDetail);
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }
}
