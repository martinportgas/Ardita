using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardita.Models.ViewModels;
using System.Reflection;

namespace Ardita.Repositories.Classess
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly BksArditaDevContext _context;

        public EmployeeRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(MstEmployee model)
        {
            int result = 0;

            if (model.EmployeeId != Guid.Empty)
            {
                var data = await _context.MstEmployees.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId);
                if (data != null)
                {
                    model.IsActive = false;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstEmployee>> GetAll()
        {
            var result = await _context.MstEmployees
                .Include(x => x.Position)
                .AsNoTracking()
                .Where(x => x.IsActive == true).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MstEmployee>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<MstEmployee> result;

            var propertyInfo = typeof(MstEmployee).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(MstEmployee).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.MstEmployees
                .Include(x => x.Position)
                .Where(
                    x => (x.Nik + x.Name).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderBy(x => EF.Property<MstEmployee>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.MstEmployees
                .Include(x => x.Position)
                .Where(
                    x => (x.Nik + x.Name).Contains(model.searchValue) &&
                    x.IsActive == true
                    )
                .OrderByDescending(x => EF.Property<MstEmployee>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<MstEmployee> GetById(Guid id)
        {
            var result = await _context.MstEmployees
                .Include(x => x.Position)
                .AsNoTracking()
                .FirstAsync(x => x.EmployeeId == id && x.IsActive == true);
            return result;
        }

        public async Task<int> GetCount()
        {
            var results = await _context.MstEmployees.AsNoTracking().Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<int> Insert(MstEmployee model)
        {
            int result = 0;
            if (model != null) 
            {
                var data = await _context.MstEmployees.AsNoTracking().FirstOrDefaultAsync(x => x.Nik == model.Nik);
                model.IsActive = true;
                if (data != null)
                {
                    model.EmployeeId = data.EmployeeId;
                    model.UpdateBy = model.CreatedBy;
                    model.UpdateDate = DateTime.Now;
                    _context.MstEmployees.Update(model);
                    result = await _context.SaveChangesAsync();
                }
                else
                {
                    _context.MstEmployees.Add(model);
                    result = await _context.SaveChangesAsync();
                }
            }
                
            return result;
        }

        public async Task<bool> InsertBulk(List<MstEmployee> employees)
        {
            bool result = false;
            if (employees.Count() > 0)
            {
                await _context.AddRangeAsync(employees);
                await _context.SaveChangesAsync();
                result = true;
            }
            return result;
        }

        public async Task<int> Update(MstEmployee model)
        {
            int result = 0;

            if (model.EmployeeId != Guid.Empty)
            {
                var data = await _context.MstEmployees.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId);
                if (data != null)
                {
                    model.IsActive = true;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    _context.MstEmployees.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
