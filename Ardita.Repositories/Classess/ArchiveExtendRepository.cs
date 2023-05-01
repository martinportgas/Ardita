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
    public class ArchiveExtendRepository : IArchiveExtendRepository
    {
        private readonly BksArditaDevContext _context;
        public ArchiveExtendRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxArchiveExtend model)
        {
            int result = 0;

            if (model.ArchiveExtendId != Guid.Empty)
            {
                var data = _context.TrxArchiveExtends.AsNoTracking().Where(x => x.ArchiveExtendId == model.ArchiveExtendId).FirstOrDefault();
                if (data != null)
                {
                    data.IsActive = false;
                    data.UpdatedBy = model.UpdatedBy;
                    data.UpdatedDate = DateTime.Now;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
        public async Task<int> Submit(TrxArchiveExtend model)
        {
            int result = 0;

            if (model.ArchiveExtendId != Guid.Empty)
            {
                var data = await _context.TrxArchiveExtends.AsNoTracking().FirstAsync(x => x.ArchiveExtendId == model.ArchiveExtendId);
                if (data != null)
                {
                    data.ApproveLevel = model.ApproveLevel;
                    data.IsActive = model.IsActive;
                    data.StatusId = model.StatusId;
                    data.UpdatedBy = model.UpdatedBy;
                    data.UpdatedDate = model.UpdatedDate;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
        public async Task<IEnumerable<TrxArchiveExtend>> GetAll()
        {
            var results = await _context.TrxArchiveExtends.Where(x => x.IsActive == true).ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxArchiveExtends.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<TrxArchiveExtend> GetById(Guid id)
        {
            var result = await _context.TrxArchiveExtends.AsNoTracking().FirstAsync(x => x.ArchiveExtendId == id);
            return result;
        }
        public async Task<IEnumerable<TrxArchiveExtend>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<TrxArchiveExtend> result;

            var propertyInfo = typeof(TrxArchiveExtend).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(TrxArchiveExtend).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.TrxArchiveExtends
                    .Include(x => x.Status)
                    .Where(x => x.IsActive == true &&
                    (
                    x.ExtendCode
                    + x.ExtendName
                    + x.Status.Name
                    )
                    .Contains(model.searchValue))
                    .OrderBy(x => EF.Property<TrxArchiveExtend>(x, propertyName))
                    .Skip(model.skip).Take(model.pageSize)
                    .ToListAsync();
            }
            else
            {
                result = await _context.TrxArchiveExtends
                    .Include(x => x.Status)
                    .Where(x => x.IsActive == true &&
                    (
                    x.ExtendCode
                    + x.ExtendName
                    + x.Status.Name
                    )
                    .Contains(model.searchValue))
                    .OrderByDescending(x => EF.Property<TrxArchiveExtend>(x, propertyName))
                    .Skip(model.skip).Take(model.pageSize)
                    .ToListAsync();
            }

            return result;
        }

        public async Task<int> Insert(TrxArchiveExtend model)
        {
            int result = 0;

            var count = await _context.TrxArchiveExtends.CountAsync();

            if (model != null)
            {
                model.IsActive = true;
                model.ExtendCode = $"EXT.{++count}/{DateTime.Now.Month}/{DateTime.Now.Year}";
                _context.TrxArchiveExtends.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxArchiveExtend> models)
        {
            bool result = false;
            if (models.Count() > 0)
            {
                await _context.BulkInsertAsync(models);
                result = true;
            }
            return result;
        }
        public async Task<int> Update(TrxArchiveExtend model)
        {
            int result = 0;

            if (model != null && model.ArchiveExtendId != Guid.Empty)
            {
                var data = await _context.TrxArchiveExtends.AsNoTracking().FirstAsync(x => x.ArchiveExtendId == model.ArchiveExtendId);
                if (data != null)
                {
                    model.ExtendCode = data.ExtendCode;
                    model.ApproveLevel = data.ApproveLevel;
                    model.ApproveMax = data.ApproveMax;
                    model.StatusId = data.StatusId;
                    model.IsActive = data.IsActive;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
