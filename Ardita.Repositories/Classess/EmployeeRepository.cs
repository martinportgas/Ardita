using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (model != null && model.EmployeeId != Guid.Empty)
            {
                _context.MstEmployees.Remove(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<MstEmployee>> GetAll()
        {
            var result = await _context.MstEmployees.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MstEmployee>> GetById(Guid id)
        {
            var result = await _context.MstEmployees.Where(x => x.EmployeeId == id).ToListAsync();
            return result;
        }

        public async Task<int> Insert(MstEmployee model)
        {
            int result = 0;

            if (model != null)
            {
                _context.MstEmployees.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(MstEmployee model)
        {
            int result = 0;

            if (model != null && model.EmployeeId != Guid.Empty)
            {
                var data = await _context.MstEmployees.Where(x => x.EmployeeId == model.EmployeeId).ToListAsync();
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
