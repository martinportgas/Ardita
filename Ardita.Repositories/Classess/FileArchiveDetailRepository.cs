using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class FileArchiveDetailRepository : IFileArchiveDetailRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public FileArchiveDetailRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }
        public async Task<IEnumerable<TrxFileArchiveDetail>> GetAll(string par = " 1=1 ")
        {
            return await _context.TrxFileArchiveDetails.Include(x => x.Archive.Creator.ArchiveUnit)
                .Where(par).AsNoTracking().ToListAsync();
        }
        public Task<TrxFileArchiveDetail> GetById(Guid id)
        {
            return _context.TrxFileArchiveDetails.AsNoTracking().Where(x => x.FileArchiveDetailId == id).FirstAsync();
        }
        public async Task<IEnumerable<TrxFileArchiveDetail>> GetByArchiveId(Guid id)
        {
            return await _context.TrxFileArchiveDetails.Where(x => x.IsActive == true && x.ArchiveId == id).ToListAsync();
        }
    }
}
