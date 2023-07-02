using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Report;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
   
    public class ReportRepository : IReportRepository
    {
        private readonly BksArditaDevContext _context;

        public ReportRepository(BksArditaDevContext context) => _context = context;

        public async Task<IEnumerable<ReportDocument>> GetReportDocument()
        {
            var result = await _context.TrxArchives
                .Include(x => x.Creator.ArchiveUnit)
                .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
                .Include(x => x.SecurityClassification)
                .Include(x => x.Gmd)
                .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room)
                .Where(x => x.IsActive == true && x.IsArchiveActive == true)
                .Select(x => new ReportDocument
                {
                   Location = x.Creator.ArchiveUnit.ArchiveUnitName,
                   Ruangan = x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.Rack.Room.RoomName,
                   AsalArsip = x.TypeSender,
                   Klasifikasi = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationName,
                   Keamanan = x.SecurityClassification.SecurityClassificationName,
                   GMD = x.Gmd.GmdName,
                   PeriodPenciptaan = x.CreatedDate,
                   PeriodInput = x.CreatedDateArchive
                })
                .ToListAsync();
            return result;
        }
    }
}
