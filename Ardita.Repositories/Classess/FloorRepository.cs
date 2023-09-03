using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class FloorRepository : IFloorRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public FloorRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }

        public async Task<int> Delete(TrxFloor model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.FloorId != Guid.Empty)
                {
                    var data = await _context.TrxFloors.AsNoTracking().FirstAsync(x => x.FloorId == model.FloorId && x.IsActive == true);
                    if (data != null)
                    {
                        model.CreatedBy = data.CreatedBy;
                        model.CreatedDate = data.CreatedDate;
                        model.IsActive = false;

                        model.ArchiveUnit = null;
                        _context.Update(model);
                        result = await _context.SaveChangesAsync();
                        //Log
                        if (result > 0)
                        {
                            try
                            {
                                await _logChangesRepository.CreateLog<TrxFloor>(GlobalConst.Delete, (Guid)model.UpdatedBy!, new List<TrxFloor> { data }, new List<TrxFloor> {  });
                            }
                            catch (Exception ex) { }
                        }
                    }
                }
                
            }
            return result;
        }

        public async Task<IEnumerable<TrxFloor>> GetAll(string par = " 1=1 ")
        {
            var results = await _context.TrxFloors
                .Include(x => x.ArchiveUnit)
                .AsNoTracking()
                .Where(x => x.IsActive == true)
                .Where(x => x.ArchiveUnit!.IsActive == true)
                .Where(par)
                .ToListAsync();
            return results;
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxFloors
               .Include(x => x.ArchiveUnit)
               .Where(x => x.IsActive == true && (x.FloorCode + x.FloorName + x.ArchiveUnit.ArchiveUnitName).Contains(model.searchValue))
               .Where(x => x.ArchiveUnit!.IsActive == true)
               .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
               .Skip(model.skip).Take(model.pageSize)
               .Select(x => new {
                   x.FloorId,
                   x.FloorCode,
                   x.FloorName,
                   x.ArchiveUnit.ArchiveUnitName
               })
               .ToListAsync();
            return result;
        }

        public async Task<TrxFloor> GetById(Guid id)
        {
            var result = await _context.TrxFloors
                .Include(x => x.ArchiveUnit)
                .AsNoTracking()
                .FirstAsync(x => x.FloorId == id && x.IsActive == true);
            return result;
        }

        public async Task<int> GetCount(DataTableModel model)
        {
            var result = await _context.TrxFloors
            .Include(x => x.ArchiveUnit)
            .Where(x => x.IsActive == true && (x.FloorCode + x.FloorName + x.ArchiveUnit.ArchiveUnitName).Contains(model.searchValue))
            .Where(x => x.ArchiveUnit!.IsActive == true)
            .CountAsync();

            return result;
        }

        public async Task<int> Insert(TrxFloor model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                model.ArchiveUnit = null;
                _context.TrxFloors.Add(model);
                result = await _context.SaveChangesAsync();
                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxFloor>(GlobalConst.New, model.CreatedBy, new List<TrxFloor> {  }, new List<TrxFloor> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<bool> InsertBulk(List<TrxFloor> floors)
        {
            bool result = false;
            if (floors.Count() > 0)
            {
                await _context.AddRangeAsync(floors);
                await _context.SaveChangesAsync();
                result = true;
                //Log
                if (result)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxFloor>(GlobalConst.New, floors.FirstOrDefault()!.CreatedBy, new List<TrxFloor> { }, floors);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<int> Update(TrxFloor model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.FloorId != Guid.Empty)
                {
                    var data = await _context.TrxFloors.AsNoTracking().FirstAsync(x => x.FloorId == model.FloorId);
                    if (data != null)
                    {
                        model.CreatedBy = data.CreatedBy;
                        model.CreatedDate = data.CreatedDate;
                        model.IsActive = true;

                        model.ArchiveUnit = null;
                        _context.Update(model);
                        result = await _context.SaveChangesAsync();

                        //Log
                        if (result > 0)
                        {
                            try
                            {
                                await _logChangesRepository.CreateLog<TrxFloor>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<TrxFloor> { data }, new List<TrxFloor> { model });
                            }
                            catch (Exception ex) { }
                        }
                    }
                }
               
            }
            return result;
        }
    }
}
