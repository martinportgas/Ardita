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
                var data = await _context.TrxArchiveMovements.AsNoTracking().FirstAsync(x => x.ArchiveMovementId == model.ArchiveMovementId);
                if (data != null)
                {
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
        public async Task<int> Submit(TrxArchiveMovement model)
        {
            int result = 0;

            if (model.ArchiveMovementId != Guid.Empty)
            {
                var data = await _context.TrxArchiveMovements.AsNoTracking().FirstAsync(x => x.ArchiveMovementId == model.ArchiveMovementId);
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

        public async Task<TrxArchiveMovement> GetById(Guid id)
        {
            var result = await _context.TrxArchiveMovements.AsNoTracking().FirstAsync(x => x.ArchiveMovementId == id);
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
                .Include(x => x.Status)
                .Where(x => x.IsActive == true &&
                (
                x.MovementCode
                + x.MovementName
                + x.Status.Name
                )
                .Contains(model.searchValue))
                .OrderBy(x => EF.Property<TrxArchiveMovement>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxArchiveMovements
                .Include(x => x.Status)
                .Where(x => x.IsActive == true &&
                (
                x.MovementCode
                + x.MovementName
                + x.Status.Name
                )
                .Contains(model.searchValue))
                .OrderByDescending(x => EF.Property<TrxArchiveMovement>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<int> Insert(TrxArchiveMovement model)
        {
            int result = 0;

            var count = await _context.TrxArchiveMovements.CountAsync();

            if (model != null)
            {
                model.IsActive = true;
                model.MovementCode = $"MOVE.{++count}/{DateTime.Now.Month}/{DateTime.Now.Year}";
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
                var data = await _context.TrxArchiveMovements.AsNoTracking().FirstAsync(x => x.ArchiveMovementId == model.ArchiveMovementId);
                if (data != null)
                {
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
