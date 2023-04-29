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
    public class FileArchiveDetailRepository : IFileArchiveDetailRepository
    {
        private readonly BksArditaDevContext _context;

        public FileArchiveDetailRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public Task<TrxFileArchiveDetail> GetById(Guid id)
        {
            return _context.TrxFileArchiveDetails.AsNoTracking().Where(x => x.IsActive == true).FirstAsync();
        }
    }
}
