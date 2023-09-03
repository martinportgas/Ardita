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
    public class LevelRepository : ILevelRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public LevelRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }

        public async Task<int> Delete(TrxLevel model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.LevelId != Guid.Empty)
                {
                    var data = await _context.TrxLevels.AsNoTracking().FirstOrDefaultAsync(x => x.LevelId == model.LevelId && x.IsActive == true);
                    if (data != null)
                    {
                        model.CreatedBy = data.CreatedBy;
                        model.CreatedDate = data.CreatedDate;
                        model.IsActive = false;

                        model.Rack = null;
                        _context.Update(model);
                        result = await _context.SaveChangesAsync();

                        //Log
                        if (result > 0)
                        {
                            try
                            {
                                await _logChangesRepository.CreateLog<TrxLevel>(GlobalConst.Delete, (Guid)model.UpdatedBy!, new List<TrxLevel> { data }, new List<TrxLevel> {  });
                            }
                            catch (Exception ex) { }
                        }
                    }
                }

            }
            return result;
        }

        public async Task<IEnumerable<TrxLevel>> GetAll(string par = " 1=1 ")
        {
            var results = await _context
                .TrxLevels
                .Include(x => x.Rack!.Room!.Floor!.ArchiveUnit)
                .Where(x => x.IsActive == true)
                .Where(x => x.Rack!.IsActive == true)
                .Where(x => x.Rack!.Room!.IsActive == true)
                .Where(x => x.Rack!.Room!.Floor!.IsActive == true)
                .Where(x => x.Rack!.Room!.Floor!.ArchiveUnit!.IsActive == true)
                .Where(par)
                .AsNoTracking()
                .ToListAsync();
            return results;
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxLevels
                .Include(x => x.Rack.Room.Floor.ArchiveUnit)
                .Where(x => x.IsActive == true && (
                                x.LevelCode +
                                x.LevelName +
                                x.Rack.RackName +
                                x.Rack.Room.RoomName +
                                x.Rack.Room.Floor.FloorName +
                                x.Rack.Room.Floor.ArchiveUnit.ArchiveUnitName
                                )
                                .Contains(model.searchValue))
                 .Where(x => x.Rack!.IsActive == true)
                .Where(x => x.Rack!.Room!.IsActive == true)
                .Where(x => x.Rack!.Room!.Floor!.IsActive == true)
                .Where(x => x.Rack!.Room!.Floor!.ArchiveUnit!.IsActive == true)
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.LevelId,
                    x.LevelCode,
                    x.LevelName,
                    x.Rack.RackName,
                    x.Rack.Room.RoomName,
                    x.Rack.Room.Floor.FloorName,
                    x.Rack.Room.Floor.ArchiveUnit.ArchiveUnitName,
                })
                .ToListAsync();

            return result;
        }

        public async Task<TrxLevel> GetById(Guid id)
        {
            var result = await _context.TrxLevels
                .Include(x => x.Rack.Room.Floor.ArchiveUnit)
                .AsNoTracking()
                .Where(x => x.LevelId == id && x.IsActive == true)
                .FirstAsync();

            return result;
        }

        public async Task<int> GetCount(DataTableModel model)
        {
            var result = await _context.TrxLevels
                .Include(x => x.Rack.Room.Floor.ArchiveUnit)
                .Where(x => x.IsActive == true && (
                                x.LevelCode +
                                x.LevelName +
                                x.Rack.RackName +
                                x.Rack.Room.RoomName +
                                x.Rack.Room.Floor.FloorName +
                                x.Rack.Room.Floor.ArchiveUnit.ArchiveUnitName
                                ).Contains(model.searchValue))
                 .Where(x => x.Rack!.IsActive == true)
                .Where(x => x.Rack!.Room!.IsActive == true)
                .Where(x => x.Rack!.Room!.Floor!.IsActive == true)
                .Where(x => x.Rack!.Room!.Floor!.ArchiveUnit!.IsActive == true)
                .CountAsync();
            return result;
               
        }

        public async Task<int> Insert(TrxLevel model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                model.Rack = null;
                _context.TrxLevels.Add(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxLevel>(GlobalConst.New, model.CreatedBy, new List<TrxLevel> {  }, new List<TrxLevel> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<bool> InsertBulk(List<TrxLevel> levels)
        {
            bool result = false;
            if (levels.Count() > 0)
            {
                await _context.AddRangeAsync(levels);
                await _context.SaveChangesAsync();
                result = true;

                //Log
                if (result)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxLevel>(GlobalConst.New, levels.FirstOrDefault()!.CreatedBy, new List<TrxLevel> { }, levels);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<int> Update(TrxLevel model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.LevelId != Guid.Empty) 
                {
                    var data = await _context.TrxLevels.AsNoTracking().FirstAsync(x => x.LevelId == model.LevelId);
                    if (data != null)
                    {
                        model.CreatedBy = data.CreatedBy;
                        model.CreatedDate = data.CreatedDate;
                        model.IsActive = true;

                        model.Rack = null;
                        _context.Update(model);
                        result = await _context.SaveChangesAsync();

                        //Log
                        if (result > 0)
                        {
                            try
                            {
                                await _logChangesRepository.CreateLog<TrxLevel>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<TrxLevel> { data }, new List<TrxLevel> { model });
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
