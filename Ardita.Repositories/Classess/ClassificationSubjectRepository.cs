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
    public class ClassificationSubjectRepository : IClassificationSubjectRepository
    {
        private readonly BksArditaDevContext _context;
        public ClassificationSubjectRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxSubjectClassification model)
        {
            int result = 0;

            if (model.SubjectClassificationId != Guid.Empty)
            {
                _context.TrxSubjectClassifications.Remove(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<TrxSubjectClassification>> GetAll()
        {
            var results = await _context.TrxSubjectClassifications.ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxSubjectClassifications.CountAsync();
            return results;
        }

        public async Task<IEnumerable<TrxSubjectClassification>> GetById(Guid id)
        {
            var result = await _context.TrxSubjectClassifications.AsNoTracking().Where(x => x.SubjectClassificationId == id).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TrxSubjectClassification>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<TrxSubjectClassification> result;

            var propertyInfo = typeof(TrxSubjectClassification).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(TrxSubjectClassification).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.TrxSubjectClassifications
                .Where(x => (x.SubjectClassificationCode + x.SubjectClassificationName).Contains(model.searchValue))
                .OrderBy(x => EF.Property<TrxSubjectClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxSubjectClassifications
                .Where(x => (x.SubjectClassificationCode + x.SubjectClassificationName).Contains(model.searchValue))
                .OrderByDescending(x => EF.Property<TrxSubjectClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<int> Insert(TrxSubjectClassification model)
        {
            int result = 0;

            if (model != null)
            {
                _context.TrxSubjectClassifications.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(TrxSubjectClassification model)
        {
            int result = 0;

            if (model != null && model.SubjectClassificationId != Guid.Empty)
            {
                var data = await _context.TrxSubjectClassifications.AsNoTracking().Where(x => x.SubjectClassificationId == model.SubjectClassificationId).ToListAsync();
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