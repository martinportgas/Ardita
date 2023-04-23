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
    public class FloorRepository : IFloorRepository
    {
        private readonly BksArditaDevContext _context;
        public FloorRepository(BksArditaDevContext context )
        {
            _context = context;
        }

        public async Task<int> Delete(TrxFloor model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.FloorId != Guid.Empty)
                {
                    var data = await _context.TrxFloors.AsNoTracking().Where(x => x.FloorId == model.FloorId && x.IsActive == true).ToListAsync();
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

        public async Task<IEnumerable<TrxFloor>> GetAll()
        {
            var results = await _context.TrxFloors.AsNoTracking().Where(x => x.IsActive == true).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<TrxFloor>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<TrxFloor> result;

            var propertyInfo = typeof(TrxFloor).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(TrxFloor).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.TrxFloors
                .Where(
                    x => (x.FloorId + x.FloorName).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderBy(x => EF.Property<TrxFloor>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxFloors
                .Where(
                    x => (x.FloorId + x.FloorName).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderByDescending(x => EF.Property<TrxClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<IEnumerable<TrxFloor>> GetById(Guid id)
        {
            var result = await _context.TrxFloors.AsNoTracking().Where(x => x.FloorId == id && x.IsActive == true).ToListAsync();
            return result;
        }

        public async Task<int> GetCount()
        {
            var results = await _context.TrxFloors.AsNoTracking().Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<int> Insert(TrxFloor model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.TrxFloors.Add(model);
                result = await _context.SaveChangesAsync();
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
                    var data = await _context.TrxFloors.AsNoTracking().Where(x => x.FloorId == model.FloorId).ToListAsync();
                    if (data != null)
                    {
                        model.CreatedBy = data.FirstOrDefault().CreatedBy;
                        model.CreatedDate = data.FirstOrDefault().CreatedDate;
                        model.IsActive = true;

                        _context.Update(model);
                        result = await _context.SaveChangesAsync();
                    }
                }
               
            }
            return result;
        }
    }
}
