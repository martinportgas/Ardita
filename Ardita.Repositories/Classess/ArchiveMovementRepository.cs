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
    public class ArchiveMovementRepository : IArchiveMovementRepository
    {
        private readonly BksArditaDevContext _context;
        public ArchiveMovementRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxArchiveMovement model)
        {
            int result = 0;

            if (model.ArchiveMovementId != Guid.Empty)
            {
                var data = _context.TrxArchiveMovements.AsNoTracking().Where(x => x.ArchiveMovementId == model.ArchiveMovementId).FirstOrDefault();
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

        public async Task<IEnumerable<TrxArchiveMovement>> GetAll()
        {
            var results = await _context.TrxArchiveMovements.Where(x => x.IsActive == true).ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxArchiveMovements.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<IEnumerable<TrxArchiveMovement>> GetById(Guid id)
        {
            var result = await _context.TrxArchiveMovements.AsNoTracking().Where(x => x.ArchiveMovementId == id).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TrxArchiveMovement>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<TrxArchiveMovement> result;

            var propertyInfo = typeof(TrxArchiveMovement).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(TrxArchiveMovement).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.TrxArchiveMovements
                //.Where(x => x.IsActive == true && x.Archive.TitleArchive.Contains(model.searchValue))
                .OrderBy(x => EF.Property<TrxArchiveMovement>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxArchiveMovements
                //.Where(x => x.IsActive == true && x.Archive.TitleArchive.Contains(model.searchValue))
                .OrderByDescending(x => EF.Property<TrxArchiveMovement>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<int> Insert(TrxArchiveMovement model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.TrxArchiveMovements.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxArchiveMovement> models)
        {
            bool result = false;
            if (models.Count() > 0)
            {
                await _context.BulkInsertAsync(models);
                result = true;
            }
            return result;
        }
        public async Task<int> Update(TrxArchiveMovement model)
        {
            int result = 0;

            if (model != null && model.ArchiveMovementId != Guid.Empty)
            {
                var data = await _context.TrxArchiveMovements.AsNoTracking().Where(x => x.ArchiveMovementId == model.ArchiveMovementId).ToListAsync();
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
