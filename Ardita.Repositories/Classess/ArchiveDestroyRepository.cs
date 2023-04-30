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

        public async Task<IEnumerable<TrxArchiveDestroy>> GetById(Guid id)
        {
            var result = await _context.TrxArchiveDestroys.AsNoTracking().Where(x => x.ArchiveDestroyId == id).ToListAsync();
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
                //.Where(x => x.IsActive == true && x.Archive.TitleArchive.Contains(model.searchValue))
                .OrderBy(x => EF.Property<TrxArchiveDestroy>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxArchiveDestroys
                //.Where(x => x.IsActive == true && x.Archive.TitleArchive.Contains(model.searchValue))
                .OrderByDescending(x => EF.Property<TrxArchiveDestroy>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<int> Insert(TrxArchiveDestroy model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
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
                var data = await _context.TrxArchiveDestroys.AsNoTracking().Where(x => x.ArchiveDestroyId == model.ArchiveDestroyId).ToListAsync();
                if (data != null)
                {
                    model.IsActive = true;
                    model.CreatedBy = data.FirstOrDefault().CreatedBy;
                    model.CreatedDate = data.FirstOrDefault().CreatedDate;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
