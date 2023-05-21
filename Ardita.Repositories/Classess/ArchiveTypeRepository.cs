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
    public class ArchiveTypeRepository : IArchiveTypeRepository
    {
        private readonly BksArditaDevContext _context;
        public ArchiveTypeRepository(BksArditaDevContext context) => _context = context;
        public async Task<int> Delete(MstArchiveType model)
        {
            int result = 0;

            if (model.ArchiveTypeId != Guid.Empty)
            {
                var data = await _context.MstArchiveTypes.AsNoTracking().FirstAsync(x => x.ArchiveTypeId == model.ArchiveTypeId);
                if (data != null)
                {
                    data.IsActive = false;
                    data.UpdatedDate = model.UpdatedDate;
                    data.UpdatedBy = model.UpdatedBy;
                    _context.MstArchiveTypes.Update(data);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstArchiveType>> GetAll() => await _context.MstArchiveTypes.Where(x => x.IsActive == true).ToListAsync();

        public async Task<IEnumerable<MstArchiveType>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<MstArchiveType> result;

            var propertyInfo = typeof(MstArchiveType).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(MstArchiveType).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.MstArchiveTypes
                .Where(x => (x.ArchiveTypeCode + x.ArchiveTypeName).Contains(model.searchValue) && x.IsActive == true)
                .OrderBy(x => EF.Property<MstArchiveType>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.MstArchiveTypes
                .Where(x => (x.ArchiveTypeCode + x.ArchiveTypeName).Contains(model.searchValue) && x.IsActive == true)
                .OrderByDescending(x => EF.Property<MstArchiveType>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<IEnumerable<MstArchiveType>> GetById(Guid id) => await _context.MstArchiveTypes.Where(x => x.ArchiveTypeId == id).ToListAsync();

        public async Task<int> GetCount() => await _context.MstArchiveTypes.CountAsync(x => x.IsActive == true);

        public async Task<int> Insert(MstArchiveType model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.MstArchiveTypes.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<bool> InsertBulk(List<MstArchiveType> MstArchiveTypes)
        {
            bool result = false;
            if (MstArchiveTypes.Count() > 0)
            {
                await _context.AddRangeAsync(MstArchiveTypes);
                await _context.SaveChangesAsync();
                result = true;
            }
            return result;
        }

        public async Task<int> Update(MstArchiveType model)
        {
            int result = 0;

            if (model != null && model.ArchiveTypeId != Guid.Empty)
            {
                var data = await _context.MstArchiveTypes.AsNoTracking().FirstAsync(x => x.ArchiveTypeId == model.ArchiveTypeId);
                if (data != null)
                {
                    model.IsActive = data.IsActive;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    _context.MstArchiveTypes.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
