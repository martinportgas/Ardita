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
        public UserArchiveUnitRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public Task<int> Delete(IdxUserArchiveUnit model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IdxUserArchiveUnit>> GetAll()
        {
            return await _context.IdxUserArchiveUnits.Include(u => u.User).Include(a=> a.ArchiveUnit).ToListAsync();
        }

        public Task<IEnumerable<IdxUserArchiveUnit>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Insert(IdxUserArchiveUnit model)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(IdxUserArchiveUnit model)
        {
            throw new NotImplementedException();
        }
    }
}
