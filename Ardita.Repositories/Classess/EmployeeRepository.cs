using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardita.Models.ViewModels;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Ardita.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ardita.Repositories.Classess
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public EmployeeRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }

        public async Task<int> Delete(MstEmployee model)
        {
            int result = 0;

            if (model.EmployeeId != Guid.Empty)
            {
                var data = await _context.MstEmployees.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeId == model.EmployeeId);
                if (data != null)
                {
                    data.IsActive = false;
                    data.UpdateBy = data.UpdateBy;
                    data.UpdateDate = data.UpdateDate;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstEmployee>(GlobalConst.Delete, (Guid)data.UpdateBy!, new List<MstEmployee> { data }, new List<MstEmployee> {  });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstEmployee>> GetAll(string par = " 1=1 ")
        {
            var result = await _context.MstEmployees
                .Include(x => x.Position)
                .AsNoTracking()
                .Where(x => x.IsActive == true)
                .Where(x => x.Position.IsActive == true)
                .Where(par)
                .AsNoTracking()
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.MstEmployees
                .Include(x => x.Position)
                .Where(
                    x => (x.Nik + x.Name).Contains(model.searchValue) &&
                    x.IsActive == true
            )
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip)
                .Take(model.pageSize)
                .Select(x => new { 
                    x.EmployeeId,
                    x.Nik,
                    x.Name,
                    x.Email,
                    x.Gender,
                    x.PlaceOfBirth,
                    x.DateOfBirth,
                    x.Address,
                    x.Phone,
                    x.EmployeeLevel,
                    PositionName = x.Position.Name
                })
                .ToListAsync();

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

        public async Task<int> GetCount(DataTableModel model)
        {
            var results = await _context.MstEmployees
                .AsNoTracking()
                .Where($"(NIK+Name).Contains(@0) and IsActive = true", model.searchValue)
                .CountAsync();
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

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstEmployee>(GlobalConst.New, data.CreatedBy, new List<MstEmployee> {  }, new List<MstEmployee> { model });
                    }
                    catch (Exception ex) { }
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

                //Log
                if (result)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstEmployee>(GlobalConst.New, employees.FirstOrDefault()!.CreatedBy, new List<MstEmployee> { }, employees);
                    }
                    catch (Exception ex) { }
                }
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
                    model.Position = null;
                    model.IsActive = true;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    _context.MstEmployees.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstEmployee>(GlobalConst.Update, (Guid)data.UpdateBy!, new List<MstEmployee> { data }, new List<MstEmployee> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
    }
}
