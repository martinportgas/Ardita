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
    public class PositionRepository : IPositionRepository
    {
        private readonly BksArditaDevContext _context;
        public PositionRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(MstPosition model)
        {
            int result = 0;

            if (model != null && model.PositionId != Guid.Empty)
            {
                var data = await _context.MstPositions.AsNoTracking().FirstAsync(x => x.PositionId == model.PositionId);
                if (data != null)
                {
                    model.IsActive = false;
                    _context.MstPositions.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstPosition>> GetAll()
        {
            var results = await _context.MstPositions.AsNoTracking().Where(x=>x.IsActive == true).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<MstPosition>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<MstPosition> result;

            var propertyInfo = typeof(MstPosition).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(MstPosition).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.MstPositions
                .Where(
                    x => (x.Name).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderBy(x => EF.Property<MstPosition>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.MstPositions
                .Where(
                    x => (x.Name).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderByDescending(x => EF.Property<MstPosition>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<MstPosition> GetById(Guid id)
        {
            var result = await _context.MstPositions.AsNoTracking().FirstAsync(x => x.PositionId == id);
            return result;
        }

        public async Task<int> GetCount()
        {
            var results = await _context.MstPositions.AsNoTracking().Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<int> Insert(MstPosition model)
        {
            int result = 0;

            if (model.PositionId == Guid.Empty)
            {
                var data = await _context.MstPositions.AsNoTracking().FirstOrDefaultAsync(x => x.Code.ToUpper() == model.Code.ToUpper());
                model.IsActive = true;
                if (data != null)
                {
                    model.PositionId = data.PositionId;
                    _context.MstPositions.Update(model);
                }
                else 
                {
                    _context.MstPositions.Add(model);
                }
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(MstPosition model)
        {
            int result = 0;

            if (model != null && model.PositionId != Guid.Empty)
            {
                var data = await _context.MstPositions.AsNoTracking().FirstOrDefaultAsync(x => x.PositionId == model.PositionId);
                if (data != null)
                {
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    model.IsActive = true;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
