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
        public RoomRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxRoom model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.RoomId != Guid.Empty)
                {
                    var data = await _context.TrxRooms.AsNoTracking().Where(x => x.RoomId == model.RoomId && x.IsActive == true).ToListAsync();
                    if (data != null)
                    {
                        model.CreatedBy = data.FirstOrDefault().CreatedBy;
                        model.CreatedDate = data.FirstOrDefault().CreatedDate;
                        model.IsActive = false;

                        _context.Update(model);
                        result = await _context.SaveChangesAsync();
                    }
                }

            }
            return result;
        }

        public async Task<IEnumerable<TrxRoom>> GetAll()
        {
            var results = await _context.TrxRooms.AsNoTracking().Where(x=>x.IsActive == true).ToListAsync();
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
                .Where(
                    x => (x.RoomId + x.RoomName).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderBy(x => EF.Property<TrxRoom>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxRooms
                .Where(
                    x => (x.RoomId + x.RoomName).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderByDescending(x => EF.Property<TrxRoom>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<IEnumerable<TrxRoom>> GetById(Guid id)
        {
            var result = await _context.TrxRooms.AsNoTracking().Where(x => x.RoomId == id && x.IsActive == true).ToListAsync();
            return result;
        }

        public async Task<int> GetCount()
        {
            var results = await _context.TrxRooms.AsNoTracking().Where(x=>x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<int> Insert(TrxRoom model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.TrxRooms.Add(model);
                result = await _context.SaveChangesAsync();
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
                    var data = await _context.TrxRooms.AsNoTracking().Where(x => x.RoomId == model.RoomId && x.IsActive == true).ToListAsync();
                    if (data != null)
                    {
                        model.CreatedBy = data.FirstOrDefault().CreatedBy;
                        model.CreatedDate = data.FirstOrDefault().CreatedDate;

                        _context.Update(model);
                        result = await _context.SaveChangesAsync();
                    }
                }
              
            }
            return result;
        }
    }
}
