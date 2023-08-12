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
    public class UserArchiveUnitRepository : IUserArchiveUnitRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public UserArchiveUnitRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }
        public Task<int> Delete(IdxUserArchiveUnit model)
        {
            throw new NotImplementedException();
        }
        public async Task<int> DeleteByUserId(Guid Id)
        {
            int result = 0;

            var data = await _context.IdxUserArchiveUnits
                .AsNoTracking()
                .Where(x => x.UserId == Id).ToListAsync();

            if (data != null)
            {
                _context.IdxUserArchiveUnits.RemoveRange(data);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<IdxUserArchiveUnit>> GetAll()
        {
            return await _context.IdxUserArchiveUnits
                .Include(u => u.User)
                .Include(a=> a.ArchiveUnit)
                .Where(x => x.User.IsActive == true)
                .Where(x => x.ArchiveUnit.IsActive == true)
                .ToListAsync();
        }

        public Task<IEnumerable<IdxUserArchiveUnit>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<IdxUserArchiveUnit>> GetByUserId(Guid id)
        {
            return await _context.IdxUserArchiveUnits.Include(x => x.ArchiveUnit).Where(x => x.UserId == id).ToListAsync();
        }

        public Task<int> Insert(IdxUserArchiveUnit model)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> InsertBulk(List<IdxUserArchiveUnit> models)
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

        public Task<int> Update(IdxUserArchiveUnit model)
        {
            throw new NotImplementedException();
        }
    }
}
