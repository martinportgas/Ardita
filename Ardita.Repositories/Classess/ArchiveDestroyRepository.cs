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
    public class ArchiveDestroyRepository : IArchiveDestroyRepository
    {
        private readonly BksArditaDevContext _context;
        public ArchiveDestroyRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxArchiveDestroy model)
        {
            int result = 0;

            if (model.ArchiveDestroyId != Guid.Empty)
            {
                var data = _context.TrxArchiveDestroys.AsNoTracking().Where(x => x.ArchiveDestroyId == model.ArchiveDestroyId).FirstOrDefault();
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
        public async Task<int> Submit(TrxArchiveDestroy model)
        {
            int result = 0;

            if (model.ArchiveDestroyId != Guid.Empty)
            {
                var data = await _context.TrxArchiveDestroys.AsNoTracking().FirstAsync(x => x.ArchiveDestroyId == model.ArchiveDestroyId);
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
        public async Task<IEnumerable<TrxArchiveDestroy>> GetAll()
        {
            var results = await _context.TrxArchiveDestroys.Where(x => x.IsActive == true).ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxArchiveDestroys.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<TrxArchiveDestroy> GetById(Guid id)
        {
            var result = await _context.TrxArchiveDestroys.AsNoTracking().FirstAsync(x => x.ArchiveDestroyId == id);
            return result;
        }
        public async Task<IEnumerable<TrxArchiveDestroy>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<TrxArchiveDestroy> result;

            var propertyInfo = typeof(TrxArchiveDestroy).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(TrxArchiveDestroy).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.TrxArchiveDestroys
                .Include(x => x.Status)
                .Where(x => x.IsActive == true &&
                (
                x.DestroyCode
                + x.DestroyName
                + x.Status.Name
                )
                .Contains(model.searchValue))
                .OrderBy(x => EF.Property<TrxArchiveDestroy>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxArchiveDestroys
                .Include(x => x.Status)
                .Where(x => x.IsActive == true &&
                (
                x.DestroyCode
                + x.DestroyName
                + x.Status.Name
                )
                .Contains(model.searchValue))
                .OrderByDescending(x => EF.Property<TrxArchiveDestroy>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<int> Insert(TrxArchiveDestroy model)
        {
            int result = 0;

            var count = await _context.TrxArchiveDestroys.CountAsync();

            if (model != null)
            {
                model.IsActive = true;
                model.DestroyCode = $"DST.{++count}/{DateTime.Now.Month}/{DateTime.Now.Year}";
                _context.TrxArchiveDestroys.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxArchiveDestroy> models)
        {
            bool result = false;
            if (models.Count() > 0)
            {
                await _context.BulkInsertAsync(models);
                result = true;
            }
            return result;
        }
        public async Task<int> Update(TrxArchiveDestroy model)
        {
            int result = 0;

            if (model != null && model.ArchiveDestroyId != Guid.Empty)
            {
                var data = await _context.TrxArchiveDestroys.AsNoTracking().FirstAsync(x => x.ArchiveDestroyId == model.ArchiveDestroyId);
                if (data != null)
                {
                    model.DestroyCode = data.DestroyCode;
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
