using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class RoomRepository : IRoomRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public RoomRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }
        public async Task<int> Delete(TrxRoom model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.RoomId != Guid.Empty)
                {
                    var data = await _context.TrxRooms.AsNoTracking().FirstAsync(x => x.RoomId == model.RoomId && x.IsActive == true);
                    if (data != null)
                    {
                        model.CreatedBy = data.CreatedBy;
                        model.CreatedDate = data.CreatedDate;
                        model.IsActive = false;

                        _context.Update(model);
                        result = await _context.SaveChangesAsync();

                        //Log
                        if (result > 0)
                        {
                            try
                            {
                                await _logChangesRepository.CreateLog<TrxRoom>(GlobalConst.Delete, (Guid)model.UpdatedBy!, new List<TrxRoom> { data }, new List<TrxRoom> {  });
                            }
                            catch (Exception ex) { }
                        }
                    }
                }

            }
            return result;
        }

        public async Task<IEnumerable<TrxRoom>> GetAll()
        {
            var results = await _context.TrxRooms
               .Include(x => x.Floor!.ArchiveUnit)
                .AsNoTracking()
                .Where(x=>x.IsActive == true)
                .Where(x=>x.Floor!.IsActive == true)
                .Where(x=>x.Floor!.ArchiveUnit!.IsActive == true)
                .ToListAsync();
            return results;
        }

        public async Task<IEnumerable<TrxRoom>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<TrxRoom> result;

            var propertyInfo = typeof(TrxRoom).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(TrxRoom).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.TrxRooms
                    .Include(x => x.Floor.ArchiveUnit)
                .Where(
                    x => (x.RoomId + x.RoomName).Contains(model.searchValue) &&
                    x.IsActive == true && x.Floor.IsActive == true
                    )
                  .Where(x => x.Floor!.IsActive == true)
                .Where(x => x.Floor!.ArchiveUnit!.IsActive == true)
                .OrderBy(x => EF.Property<TrxRoom>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxRooms
                    .Include(x => x.Floor.ArchiveUnit)
                .Where(
                    x => (x.RoomId + x.RoomName).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                  .Where(x => x.Floor!.IsActive == true)
                .Where(x => x.Floor!.ArchiveUnit!.IsActive == true)
                .OrderByDescending(x => EF.Property<TrxRoom>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<TrxRoom> GetById(Guid id)
        {
            var result = await _context.TrxRooms
                .Include(x => x.Floor.ArchiveUnit)
                .AsNoTracking().FirstAsync(x => x.RoomId == id && x.IsActive == true);
            return result;
        }

        public async Task<int> GetCount()
        {
            var results = await _context.TrxRooms.AsNoTracking().Include(x => x.Floor.ArchiveUnit).Where(x=>x.IsActive == true).Where(x => x.Floor!.IsActive == true)
                .Where(x => x.Floor!.ArchiveUnit!.IsActive == true).CountAsync();
            return results;
        }

        public async Task<int> Insert(TrxRoom model)
        {
            int result = 0;

            if (model != null)
            {
                model.Floor = null;
                model.IsActive = true;
                _context.TrxRooms.Add(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxRoom>(GlobalConst.New, model.CreatedBy, new List<TrxRoom> {  }, new List<TrxRoom> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<bool> InsertBulk(List<TrxRoom> rooms)
        {
            bool result = false;
            if (rooms.Count() > 0)
            {
                await _context.AddRangeAsync(rooms);
                await _context.SaveChangesAsync();
                result = true;

                //Log
                if (result)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxRoom>(GlobalConst.New, rooms.FirstOrDefault()!.CreatedBy, new List<TrxRoom> { }, rooms);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<int> Update(TrxRoom model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.RoomId != Guid.Empty) 
                {
                    var data = await _context.TrxRooms.AsNoTracking().FirstAsync(x => x.RoomId == model.RoomId && x.IsActive == true);
                    if (data != null)
                    {
                        model.IsActive = true;
                        model.CreatedBy = data.CreatedBy;
                        model.CreatedDate = data.CreatedDate;

                        model.Floor = null;
                        _context.Update(model);
                        result = await _context.SaveChangesAsync();

                        //Log
                        if (result > 0)
                        {
                            try
                            {
                                await _logChangesRepository.CreateLog<TrxRoom>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<TrxRoom> { data }, new List<TrxRoom> { model });
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
