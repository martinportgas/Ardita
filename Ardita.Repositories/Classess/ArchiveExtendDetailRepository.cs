﻿using Ardita.Models.DbModels;
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
    public class ArchiveExtendDetailRepository : IArchiveExtendDetailRepository
    {
        private readonly BksArditaDevContext _context;
        public ArchiveExtendDetailRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxArchiveExtendDetail model)
        {
            int result = 0;

            if (model.ArchiveExtendDetailId != Guid.Empty)
            {
                var data = _context.TrxArchiveExtendDetails.AsNoTracking().Where(x => x.ArchiveExtendDetailId == model.ArchiveExtendDetailId).FirstOrDefault();
                if (data != null)
                {
                    data.IsActive = false;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
        public async Task<int> DeleteByMainId(Guid id)
        {
            int result = 0;
            if (id != null)
            {
                _context.Database.ExecuteSqlRaw($" delete from dbo.TRX_ARCHIVE_EXTEND_DETAIL where archive_extend_id='{id}'");
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<IEnumerable<TrxArchiveExtendDetail>> GetAll()
        {
            var results = await _context.TrxArchiveExtendDetails.Include(x => x.ArchiveExtend).Where(x => x.IsActive == true && x.ArchiveExtend.StatusId != 4).ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxArchiveExtendDetails.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<IEnumerable<TrxArchiveExtendDetail>> GetById(Guid id)
        {
            var result = await _context.TrxArchiveExtendDetails.AsNoTracking().Where(x => x.ArchiveExtendDetailId == id).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TrxArchiveExtendDetail>> GetByMainId(Guid id)
        {
            var result = await _context.TrxArchiveExtendDetails.Include(x => x.Archive).AsNoTracking().Where(x => x.ArchiveExtendId == id).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TrxArchiveExtendDetail>> GetByFilterModel(DataTableModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Insert(TrxArchiveExtendDetail model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.TrxArchiveExtendDetails.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxArchiveExtendDetail> models)
        {
            bool result = false;
            if (models.Count() > 0)
            {
                await _context.AddRangeAsync(models);
                await _context.SaveChangesAsync();
                result = true;
            }
            return result;
        }
        public async Task<int> Update(TrxArchiveExtendDetail model)
        {
            int result = 0;

            if (model != null && model.ArchiveExtendDetailId != Guid.Empty)
            {
                var data = await _context.TrxArchiveExtendDetails.AsNoTracking().Where(x => x.ArchiveExtendDetailId == model.ArchiveExtendDetailId).ToListAsync();
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
