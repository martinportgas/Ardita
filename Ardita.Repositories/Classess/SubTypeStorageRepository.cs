﻿using Ardita.Extensions;
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
    private readonly ILogChangesRepository _logChangesRepository;
    public SubTypeStorageRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
    {
        _context = context;
        _logChangesRepository = logChangesRepository;
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

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstSubTypeStorage>(GlobalConst.Delete, (Guid)model.CreatedBy!, new List<MstSubTypeStorage> { data }, new List<MstSubTypeStorage> {  });
                }
                catch (Exception ex) { }
            }
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

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<IdxSubTypeStorage>(GlobalConst.Delete, data.FirstOrDefault().CreatedBy!, data, new List<IdxSubTypeStorage> { });
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }

    public async Task<int> DeleteGMDSubTypeStorage(Guid id)
    {
        int result = 0;

        var data = await _context.MstSubTypeStorageDetails
            .AsNoTracking()
            .Where(x => x.SubTypeStorageId == id).ToListAsync();

        if (data != null)
        {
            _context.MstSubTypeStorageDetails.RemoveRange(data);
            result = await _context.SaveChangesAsync();

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstSubTypeStorageDetail>(GlobalConst.Delete, (Guid)data.FirstOrDefault().CreatedBy!, data, new List<MstSubTypeStorageDetail> { });
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }
    public async Task<IEnumerable<MstSubTypeStorage>> GetAll(string par = " 1=1 ")
    {
        return await _context.MstSubTypeStorages
            .Include(x => x.IdxSubTypeStorages)
            .ThenInclude(x => x.TypeStorage)
            .Where(x => x.IsActive == true)
            .Where(par)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<MstSubTypeStorageDetail>> GetAllDetailBySubTypeStorageId(Guid ID)
    {
        var result = await _context.MstSubTypeStorageDetails
        .Include(x => x.SubTypeStorage)
        .Include(x => x.GmdDetail.Gmd)
        .Where(x => x.SubTypeStorageId == ID)
        .ToListAsync();

        return result;
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
    public async Task<IEnumerable<MstSubTypeStorage>> GetAllByTypeStorageAndGMDDetailId(Guid ID, Guid GMDDetailID)
    {
        var result = from idx in _context.IdxSubTypeStorages
                     join mst in _context.MstSubTypeStorages on idx.SubTypeStorageId equals mst.SubTypeStorageId
                     join mstdtl in _context.MstSubTypeStorageDetails on idx.SubTypeStorageId equals mstdtl.SubTypeStorageId
                     join trx in _context.TrxTypeStorages on idx.TypeStorageId equals trx.TypeStorageId
                     where idx.TypeStorageId == ID && mstdtl.GmdDetailId == GMDDetailID
                     select new MstSubTypeStorage { 
                         SubTypeStorageId = mst.SubTypeStorageId,
                         SubTypeStorageCode = mst.SubTypeStorageCode, 
                         SubTypeStorageName = mst.SubTypeStorageName, 
                         Volume = mstdtl.Size
                     };

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
            .Include(x => x.MstSubTypeStorageDetails).ThenInclude(x => x.GmdDetail)
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

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstSubTypeStorage>(GlobalConst.New, model.CreatedBy!, new List<MstSubTypeStorage> { }, new List<MstSubTypeStorage> { model});
                    }
                    catch (Exception ex) { }
                }
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

            //Log
            if (result)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstSubTypeStorage>(GlobalConst.New, mstSubTypeStorages.FirstOrDefault()!.CreatedBy, new List<MstSubTypeStorage> { }, mstSubTypeStorages);
                }
                catch (Exception ex) { }
            }
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

            //Log
            if (result)
            {
                try
                {
                    await _logChangesRepository.CreateLog<IdxSubTypeStorage>(GlobalConst.New, idxSubTypeStorages.FirstOrDefault()!.CreatedBy, new List<IdxSubTypeStorage> { }, idxSubTypeStorages);
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }
    public async Task<bool> InsertBulkGMDTypeStorage(List<MstSubTypeStorageDetail> MstSubTypeStorageDetail)
    {
        bool result = false;
        if (MstSubTypeStorageDetail.Count() > 0)
        {
            await _context.AddRangeAsync(MstSubTypeStorageDetail);
            await _context.SaveChangesAsync();
            result = true;

            //Log
            if (result)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstSubTypeStorageDetail>(GlobalConst.New, (Guid)MstSubTypeStorageDetail.FirstOrDefault().CreatedBy!, new List<MstSubTypeStorageDetail> { }, MstSubTypeStorageDetail);
                }
                catch (Exception ex) { }
            }
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

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<IdxSubTypeStorage>(GlobalConst.New, model.CreatedBy, new List<IdxSubTypeStorage> { }, new List<IdxSubTypeStorage> { model });
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }
    public async Task<int> InsertGMDSubTypeStorage(MstSubTypeStorageDetail model)
    {
        int result = 0;

        if (model != null)
        {
            _context.MstSubTypeStorageDetails.Add(model);
            result = await _context.SaveChangesAsync();

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstSubTypeStorageDetail>(GlobalConst.New, (Guid)model.CreatedBy!, new List<MstSubTypeStorageDetail> { }, new List<MstSubTypeStorageDetail> { model });
                }
                catch (Exception ex) { }
            }
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

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstSubTypeStorage>(GlobalConst.Update, (Guid)model.CreatedBy!, new List<MstSubTypeStorage> { data }, new List<MstSubTypeStorage> { model });
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }

}
