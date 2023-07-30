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
    public class RackRepository : IRackRepository
    {
        private readonly BksArditaDevContext _context;
        public RackRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxRack model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.RoomId != Guid.Empty)
                {
                    var data = await _context.TrxRacks.AsNoTracking().FirstAsync(x => x.RackId == model.RackId && x.IsActive == true);
                    if (data != null)
                    {
                        model.CreatedBy = data.CreatedBy;
                        model.CreatedDate = data.CreatedDate;
                        model.IsActive = false;

                        _context.Update(model);
                        result = await _context.SaveChangesAsync();
                    }
                }

            }
            return result;
        }

        public async Task<IEnumerable<TrxRack>> GetAll()
        {
            var results = await _context.TrxRacks
                .Include(x => x.Room!.Floor!.ArchiveUnit)
                .AsNoTracking()
                .Where(x=>x.IsActive == true)
                .Where(x=>x.Room!.IsActive == true)
                .Where(x=>x.Room!.Floor!.IsActive == true)
                .Where(x=>x.Room!.Floor!.ArchiveUnit!.IsActive == true)
                .ToListAsync();

            return results;
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxRacks
                 .Include(x => x.Room.Floor.ArchiveUnit)
                 .Where(x => x.IsActive == true && (x.RackCode + x.RackName + x.Length + x.Room.RoomName + x.Room.ArchiveRoomType + x.Room.Floor.FloorName + x.Room.Floor.ArchiveUnit.ArchiveUnitName).Contains(model.searchValue))
                 .Where(x => x.Room!.IsActive == true)
                 .Where(x => x.Room!.Floor!.IsActive == true)
                 .Where(x => x.Room!.Floor!.ArchiveUnit!.IsActive == true)
                 .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                 .Skip(model.skip).Take(model.pageSize)
                 .Select(x => new {
                     x.RackId,
                     x.RackCode,
                     x.RackName,
                     x.Length,
                     x.Room.RoomName,
                     x.Room.ArchiveRoomType,
                     x.Room.Floor.FloorName,
                     x.Room.Floor.ArchiveUnit.ArchiveUnitName
                 })
                 .ToListAsync();

            return result;
        }

        public async Task<TrxRack> GetById(Guid id)
        {
            var result = await _context.TrxRacks
                .Include(x => x.Room.Floor.ArchiveUnit)
                .AsNoTracking()
                .FirstAsync(x => x.RackId == id && x.IsActive == true);

            return result;
        }

        public async Task<int> GetCount(DataTableModel model)
        {
            var result = await _context.TrxRacks
                .Include(x => x.Room.Floor.ArchiveUnit)
                .Where(x => x.IsActive == true && (x.RackCode + x.RackName + x.Length + x.Room.RoomName + x.Room.ArchiveRoomType + x.Room.Floor.FloorName + x.Room.Floor.ArchiveUnit.ArchiveUnitName).Contains(model.searchValue))
                .Where(x => x.Room!.IsActive == true)
                .Where(x => x.Room!.Floor!.IsActive == true)
                .Where(x => x.Room!.Floor!.ArchiveUnit!.IsActive == true)
                .CountAsync();

            return result;
        }

        public async Task<int> Insert(TrxRack model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                model.Room = null;
                _context.TrxRacks.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<bool> InsertBulk(List<TrxRack> racks)
        {
            bool result = false;
            if (racks.Count() > 0)
            {
                await _context.AddRangeAsync(racks);
                await _context.SaveChangesAsync();
                result = true;
            }
            return result;
        }

        public async Task<int> Update(TrxRack model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.RackId != Guid.Empty)
                {
                    var data = await _context.TrxRacks.AsNoTracking().FirstAsync(x => x.RackId == model.RackId && x.IsActive == true);
                    if (data != null)
                    {
                        model.IsActive = true;
                        model.CreatedBy = data.CreatedBy;
                        model.CreatedDate = data.CreatedDate;

                        model.Room = null;
                        _context.Update(model);
                        result = await _context.SaveChangesAsync();
                    }
                }
            }
           
            return result;
        }
    }
}
