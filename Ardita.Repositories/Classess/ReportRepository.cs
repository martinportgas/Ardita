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

        public async Task<IEnumerable<ArchiveActive>> GetArchiveActives()
        {
            var result = await _context.TrxArchives
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room)
              .Where(x => x.IsActive == true && x.IsArchiveActive == true)
              .Where(x => x.TrxMediaStorageDetails != null)
              .Select(x => new ArchiveActive
              {
                  DocumentNo = x.DocumentNo,
                  ItemArchiveNo = x.ArchiveCode,
                  ClassificationCode = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationCode,
                  ArchiveTitle = x.TitleArchive,
                  ArchiveDescription = x.ArchiveDescription,
                  ArchiveDate = x.CreatedDateArchive,
                  ArchiveTotal = x.Volume,
                  MediaStorageCode = x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.MediaStorageCode
              })
              .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<ArchiveDestroy>> GetArchiveDestroys()
        {
            var result = await _context.TrxArchives
              .Include(x => x.Creator.ArchiveUnit.Company)
              .Include(x => x.ArchiveOwner)
              .Include(x => x.TrxFileArchiveDetails)
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.ArchiveType)
              .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.ArchiveDestroy)
              .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage)
              .Where(x => x.IsActive == true && x.IsArchiveActive == true && x.TrxArchiveDestroyDetails != null)
              .Select(x => new ArchiveDestroy
              {
                  Perusahaan = x.Creator.ArchiveUnit.Company.CompanyName,
                  AsalArsip = x.ArchiveOwner.ArchiveOwnerName,
                  PenciptaArsip = x.Creator.CreatorName,
                  Period = x.CreatedDateArchive,
                  Jumlah = x.Volume.ToString(),
                  SifatArsip = x.Description,
                  KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationCode,
                  NomorArsip = x.DocumentNo,
                  TipeArsip = x.ArchiveType.ArchiveTypeName,
                  RetensiInAktif = x.ActiveRetention.ToString(),
                  KodeMediaSimpan = x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.MediaStorageCode

              })
              .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<ArchiveMovement>> GetArchiveMovements()
        {
            var result = await _context.TrxArchives
              .Include(x => x.Creator.ArchiveUnit.Company)
              .Include(x => x.ArchiveOwner)
              .Include(x => x.TrxFileArchiveDetails)
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.ArchiveType)
              .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.ArchiveMovement)
              .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage)
              .Where(x => x.IsActive == true && x.IsArchiveActive == true && x.TrxArchiveMovementDetails != null)
              .Select(x => new ArchiveMovement
              {
                  Perusahaan = x.Creator.ArchiveUnit.Company.CompanyName,
                  AsalArsip = x.ArchiveOwner.ArchiveOwnerName,
                  PenciptaArsip = x.Creator.CreatorName,
                  Period = x.CreatedDateArchive,
                  Jumlah = x.Volume.ToString(),
                  TanggalPindah = x.CreatedDate,
                  KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationCode,
                  NomorArsip = x.DocumentNo,
                  TipeArsip = x.ArchiveType.ArchiveTypeName,
                  RetensiInAktif = x.ActiveRetention.ToString(),
                  KodeMediaSimpan = x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.MediaStorageCode

              })
              .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<ArchiveUsed>> GetArchiveUseds()
        {
            var result = await _context.TrxArchives
            .Include(x => x.TrxArchiveOutIndicators)
            .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
            .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage)
            .Where(x => x.IsActive == true && x.IsArchiveActive == true)
            .Select(x => new ArchiveUsed
            {
                NoDocumen = x.DocumentNo,
                NoItemArsip = x.ArchiveCode,
                KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationCode,
                JudulArsip = x.TitleArchive,
                UraianInformasiArsip = x.ArchiveDescription,
                Tanggal = x.CreatedDateArchive,
                Jumlah = x.Volume.ToString(),
                KodeMediaSimpan = x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.MediaStorageCode
            })
            .ToListAsync();
            return result;
        }

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

        public async Task<IEnumerable<TransferMedia>> GetTransferMedias()
        {
            var result = await _context.TrxArchives
              .Include(x => x.Creator.ArchiveUnit.Company)
              .Include(x => x.ArchiveOwner)
              .Include(x => x.TrxFileArchiveDetails)
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.ArchiveType)
              .Where(x => x.IsActive == true && x.IsArchiveActive == true)
              .Select(x => new TransferMedia
              {
                 Perusahaan = x.Creator.ArchiveUnit.Company.CompanyName,
                 AsalArsip = x.ArchiveOwner.ArchiveOwnerName,
                 PenciptaArsip = x.Creator.CreatorName,
                 Period = x.CreatedDateArchive,
                 Jumlah = x.Volume.ToString(),
                 PeriodPindai = x.CreatedDate,
                 KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationCode,
                 NomorArsip = x.DocumentNo,
                 TipeArsip = x.ArchiveType.ArchiveTypeName
              })
              .ToListAsync();
            return result;
        }
    }
}
