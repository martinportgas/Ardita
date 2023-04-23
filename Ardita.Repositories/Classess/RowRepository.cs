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
    public class RowRepository : IRowRepository
    {
        private readonly BksArditaDevContext _context;

        public RowRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(TrxRow model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.RowId != Guid.Empty)
                {
                    var data = await _context.TrxRows.AsNoTracking().Where(x => x.RowId == model.RowId && x.IsActive == true).ToListAsync();
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

        public async Task<IEnumerable<TrxRow>> GetAll()
        {
            var results = await _context.TrxRows.AsNoTracking().Where(x => x.IsActive == true).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<TrxRow>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<TrxRow> result;

            var propertyInfo = typeof(TrxLevel).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(TrxLevel).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.TrxRows
                .Where(
                    x => (x.RowId + x.RowName).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderBy(x => EF.Property<TrxRow>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxRows
                .Where(
                    x => (x.RowId + x.RowName).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderByDescending(x => EF.Property<TrxClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<IEnumerable<TrxRow>> GetById(Guid id)
        {
            var result = await _context.TrxRows.AsNoTracking().Where(x => x.RowId == id && x.IsActive == true).ToListAsync();
            return result;
        }

        public async Task<int> GetCount()
        {
            var results = await _context.TrxRows.AsNoTracking().Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<int> Insert(TrxRow model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.TrxRows.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(TrxRow model)
        {
            int result = 0;

            if (model != null)
            {
                if (model.RowId != Guid.Empty)
                {
                    var data = await _context.TrxRows.AsNoTracking().Where(x => x.RowId == model.RowId).ToListAsync();
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