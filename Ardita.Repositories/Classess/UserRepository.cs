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
    public class UserRepository : IUserRepository
    {
        private readonly BksArditaDevContext _context;
        public UserRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(MstUser model)
        {
            int result = 0;
            if (model != null && model.UserId != Guid.Empty)
            {
                _context.MstUsers.Remove(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<MstUser>> GetAll()
        {
            var result = await _context.MstUsers.ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MstUser>> GetById(Guid id)
        {
            var result = await _context.MstUsers.AsNoTracking().Where(x => x.UserId == id).ToListAsync();
            return result;
        }

        public async Task<int> Insert(MstUser model)
        {
            int result = 0;
            var emplyoee = await _context.MstUsers.Where(x => x.EmployeeId == model.EmployeeId).ToListAsync();
            if (emplyoee.Count == 0) 
            {
                _context.MstUsers.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(MstUser model)
        {
            int result = 0;

            _context.Update(model);
            result = await _context.SaveChangesAsync();
            return result;
        }

        public void Upload(MstUser model)
        {
            var emplyoee = _context.MstUsers.Where(x => x.EmployeeId == model.EmployeeId).ToList();
            if (emplyoee.Count == 0)
            {
                _context.MstUsers.Add(model);
                _context.SaveChanges();
            }
        }
    }
}
