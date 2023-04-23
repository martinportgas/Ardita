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
    public class LevelRepository : ILevelRepository
    {
        private readonly BksArditaDevContext _context;

        public LevelRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(TrxLevel model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.LevelId != Guid.Empty)
                {
                    var data = await _context.TrxFloors.AsNoTracking().Where(x => x.FloorId == model.LevelId && x.IsActive == true).ToListAsync();
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

        public async Task<IEnumerable<TrxLevel>> GetAll()
        {
            var results = await _context.TrxLevels.AsNoTracking().Where(x => x.IsActive == true).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<TrxLevel>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<TrxLevel> result;

            var propertyInfo = typeof(TrxLevel).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(TrxLevel).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.TrxLevels
                .Where(
                    x => (x.LevelId + x.LevelName).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderBy(x => EF.Property<TrxLevel>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxLevels
                .Where(
                    x => (x.LevelId + x.LevelName).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderByDescending(x => EF.Property<TrxClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<IEnumerable<TrxLevel>> GetById(Guid id)
        {
            var result = await _context.TrxLevels.AsNoTracking().Where(x => x.LevelId == id && x.IsActive == true).ToListAsync();
            return result;
        }

        public async Task<int> GetCount()
        {
            var results = await _context.TrxLevels.AsNoTracking().Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<int> Insert(TrxLevel model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.TrxLevels.Add(model);
                result = await _context.SaveChangesAsync();
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
                    var data = await _context.TrxLevels.AsNoTracking().Where(x => x.LevelId == model.LevelId).ToListAsync();
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
