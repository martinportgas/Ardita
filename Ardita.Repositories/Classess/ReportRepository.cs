using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ReportModels;
using Ardita.Report;
using Ardita.Repositories.Interfaces;
using Castle.Components.DictionaryAdapter.Xml;
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

        public async Task<IEnumerable<ReportArchiveLoansInActive>> GetReportArchiveLoansInActive()
        {
            var query = await _context.TrxArchives
                .Include(x => x.Creator)
                .Include(x => x.ArchiveOwner)
                .Include(x => x.TrxFileArchiveDetails)
                .Include(x => x.SubSubjectClassification)
                    .ThenInclude(x => x.SubjectClassification)
                .Include(x => x.ArchiveType)
                .Include(x => x.TrxArchiveRentDetails)
                    .ThenInclude(x => x.TrxArchiveRent.TrxRentHistories)
                    .ThenInclude(x => x.Borrower)
                .AsNoTracking()
                .Where(x => x.IsActive == true && x.IsArchiveActive == true && x.TrxArchiveRentDetails.FirstOrDefault().TrxArchiveRentId != null && x.TrxArchiveRentDetails.FirstOrDefault().TrxArchiveRent.ReturnDate != null)
                .Select(x => new ReportArchiveLoansInActive
                {
                    PemilikArsip = x.ArchiveOwner.ArchiveOwnerName,
                    AsalArsip = x.TypeSender,
                    UnitPencipta = x.Creator.CreatorName,
                    Jumlah = x.Volume,
                    KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.SubjectClassificationCode ?? string.Empty,
                    NoArsip = x.DocumentNo,
                    JenisArsip = x.ArchiveType.ArchiveTypeName,
                    NamaPeminjam = x.TrxArchiveRentDetails.FirstOrDefault().TrxArchiveRent.TrxRentHistories.FirstOrDefault().Borrower.BorrowerName,
                    Perusahaan = x.TrxArchiveRentDetails.FirstOrDefault().TrxArchiveRent.TrxRentHistories.FirstOrDefault().Borrower.BorrowerCompany,
                    UnitKerja = x.TrxArchiveRentDetails.FirstOrDefault().TrxArchiveRent.TrxRentHistories.FirstOrDefault().Borrower.BorrowerArchiveUnit,
                    TanggalPinjam = x.TrxArchiveRentDetails.FirstOrDefault().TrxArchiveRent.ApprovalDate,
                    TanggalKembali = x.TrxArchiveRentDetails.FirstOrDefault().TrxArchiveRent.ReturnDate,
                    Period = x.CreatedDateArchive
                }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ReportArchiveProcessingInActive>> GetReportArchiveProcessingInActive()
        {
            var query = await _context.TrxArchives
                .Include(x => x.Creator)
                .Include(x => x.ArchiveOwner)
                .Include(x => x.SubSubjectClassification)
                    .ThenInclude(x => x.SubjectClassification)
                .Include(x => x.ArchiveType)
                .Include(x => x.TrxMediaStorageInActiveDetails)
                    .ThenInclude(x => x.MediaStorageInActive)
                .Include(x => x.TrxArchiveMovementDetails)
                    .ThenInclude(x => x.ArchiveMovement)
                .Include(y => y.TrxArchiveMovementDetails)
                    .ThenInclude(y => y.ArchiveMovement.ArchiveUnitIdDestinationNavigation)
                .AsNoTracking()
                .Where(x => x.IsActive == true && x.IsArchiveActive == true && x.TrxMediaStorageInActiveDetails != null && x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.StatusReceived == 6 && x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement != null)
                .Select(x => new ReportArchiveProcessingInActive
                {
                    PemilikArsip = x.ArchiveOwner.ArchiveOwnerName,
                    AsalArsip = x.TypeSender,
                    UnitPencipta = x.Creator.CreatorName,
                    Jumlah = x.Volume,
                    TanggalDiterima = x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.DateReceived,
                    KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.SubjectClassificationCode ?? string.Empty,
                    NoArsip = x.DocumentNo,
                    JenisArsip = x.ArchiveType.ArchiveTypeName,
                    TahunPenciptaan = x.CreatedDate,
                    Retensi = x.InactiveRetention,
                    Lokasi = x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestinationNavigation.ArchiveUnitName,
                    KodeMediaSimpan = x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.MediaStorageCode,
                    PeriodeOlah = x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.CreatedDate
                }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ReportArchiveReceivedInActive>> GetReportArchiveReceivedInActive()
        {
            var query = await _context.TrxArchives
                .Include(x => x.Creator)
                .Include(x => x.ArchiveOwner)
                .Include(x => x.TrxArchiveMovementDetails)
                    .ThenInclude(x => x.ArchiveMovement)
                .Include(y => y.TrxArchiveMovementDetails).
                    ThenInclude(y => y.ArchiveMovement.CreatedByNavigation)
                .Include(z => z.TrxArchiveMovementDetails).
                    ThenInclude(z => z.ArchiveMovement.CreatedByNavigation.Employee)
                .Include(w => w.TrxArchiveMovementDetails).
                    ThenInclude(w => w.ArchiveMovement.ReceivedByNavigation)
                .Include(p => p.TrxArchiveMovementDetails).
                    ThenInclude(p => p.ArchiveMovement.ReceivedByNavigation.Employee)
                .AsNoTracking()
                .Where(x => x.IsActive == true && x.IsArchiveActive == true && x.TrxArchiveMovementDetails != null)
                .Select(x => new ReportArchiveReceivedInActive
                {
                    PemilikArsip = x.ArchiveOwner.ArchiveOwnerName,
                    AsalArsip = x.TypeSender,
                    Pengirim = x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.CreatedByNavigation.Employee.Name,
                    Penerima = x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ReceivedByNavigation.Employee.Name,
                    Tanggal = x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.DateReceived,
                    DokumenSerahTerima = x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.DocumentCode,
                    NoArsip = x.DocumentNo
                }).ToListAsync();

            return query;
        }
        public async Task<Dictionary<string, string>> GetArchiveActiveNamingParams(ReportGlobalParams param)
        {
            await Task.Delay(0);
            var parameters = new Dictionary<string, string>();
            parameters.Add("company", param.companyId == Guid.Empty ? GlobalConst.SelectAll : _context.MstCompanies.FirstOrDefault(x => x.CompanyId == param.companyId).CompanyName);
            parameters.Add("unitArchive", param.archiveUnitId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxArchiveUnits.FirstOrDefault(x => x.ArchiveUnitId == param.archiveUnitId).ArchiveUnitName);
            parameters.Add("room", param.roomId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxRooms.FirstOrDefault(x => x.RoomId == param.roomId).RoomName);
            parameters.Add("rack", param.rackId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxRacks.FirstOrDefault(x => x.RackId == param.rackId).RackName);
            parameters.Add("level", param.levelId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxLevels.FirstOrDefault(x => x.LevelId == param.levelId).LevelName);
            parameters.Add("row", param.rowId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxRows.FirstOrDefault(x => x.RowId == param.rowId).RowName);
            parameters.Add("status", param.status == null ? GlobalConst.SelectAll : param.status == true ? GlobalConst.Used : GlobalConst.Available);
            parameters.Add("gmd", param.gmdId == Guid.Empty ? GlobalConst.SelectAll : _context.MstGmds.FirstOrDefault(x => x.GmdId == param.gmdId).GmdName);
            parameters.Add("creator", param.creatorId == Guid.Empty ? GlobalConst.SelectAll : _context.MstCreators.FirstOrDefault(x => x.CreatorId == param.creatorId).CreatorName);
            parameters.Add("owner", param.archiveOwnerId == Guid.Empty ? GlobalConst.SelectAll : _context.MstArchiveOwners.FirstOrDefault(x => x.ArchiveOwnerId == param.archiveOwnerId).ArchiveOwnerName);
            parameters.Add("classification", param.classificationId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxClassifications.FirstOrDefault(x => x.ClassificationId == param.classificationId).ClassificationName);
            parameters.Add("subjectClassification", param.subjectClassificationId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxSubjectClassifications.FirstOrDefault(x => x.SubjectClassificationId == param.subjectClassificationId).SubjectClassificationName);
            parameters.Add("createPeriode", (param.startDate == null ? "-" : ((DateTime)param.startDate).ToString("dd-MM-yyyy")) + " s/d " + (param.endDate == null ? "-" : ((DateTime)param.endDate).ToString("dd-MM-yyyy")));
            parameters.Add("inputPeriode", (param.startDateCreated == null ? "-" : ((DateTime)param.startDateCreated).ToString("dd-MM-yyyy")) + " s/d " + (param.endDateCreated == null ? "-" : ((DateTime)param.endDateCreated).ToString("dd-MM-yyyy")));
            parameters.Add("destroyPeriode", (param.startDateDestroy == null ? "-" : ((DateTime)param.startDateDestroy).ToString("dd-MM-yyyy")) + " s/d " + (param.endDateDestroy == null ? "-" : ((DateTime)param.endDateDestroy).ToString("dd-MM-yyyy")));
            parameters.Add("typeStorage", param.typeStorageId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxTypeStorages.FirstOrDefault(x => x.TypeStorageId == param.typeStorageId).TypeStorageName);
            return parameters;
        }

        public async Task<IEnumerable<ArchiveActive>> GetArchiveActives(ReportGlobalParams param)
        {
            var result = await _context.TrxArchives.AsNoTracking()
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room)
              .Include(x => x.Creator.ArchiveUnit.Company)
              .Include(x => x.Gmd)
              .Include(x => x.ArchiveOwner)
              .Where(x => x.IsActive == true && x.IsArchiveActive == true)
              .Where(x => x.SubSubjectClassification != null)
              .Where(x => (param.companyId == Guid.Empty ? true : x.Creator.ArchiveUnit.CompanyId == param.companyId))
              .Where(x => (param.archiveUnitId == Guid.Empty ? true : x.Creator.ArchiveUnitId == param.archiveUnitId))
              .Where(x => (param.roomId == Guid.Empty ? true : x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.Rack.RoomId == param.roomId))
              .Where(x => (param.rackId == Guid.Empty ? true : x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.RackId == param.rackId))
              .Where(x => (param.levelId == Guid.Empty ? true : x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.LevelId == param.levelId))
              .Where(x => (param.rowId == Guid.Empty ? true : x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.RowId == param.rowId))
              .Where(x => (param.gmdId == Guid.Empty ? true : x.GmdId == param.gmdId))
              .Where(x => (param.typeStorageId == Guid.Empty ? true : x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.TypeStorageId == param.typeStorageId))
              .Where(x => (param.creatorId == Guid.Empty ? true : x.CreatorId == param.creatorId))
              .Where(x => (param.archiveOwnerId == Guid.Empty ? true : x.ArchiveOwnerId == param.archiveOwnerId))
              .Where(x => (param.classificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassification.ClassificationId == param.classificationId))
              .Where(x => (param.subjectClassificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassificationId == param.subjectClassificationId))
              .Where(x => (param.status == null ? true : x.IsUsed == param.status))
              .Where(x => (param.startDate == null ? true : x.CreatedDateArchive >= param.startDate))
              .Where(x => (param.endDate == null ? x.CreatedDateArchive <= DateTime.Now : x.CreatedDateArchive <= param.endDate))
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

        public async Task<IEnumerable<ReportListArchiveInActive>> GetReportListArchiveInActive()
        {
            var query = await _context.TrxArchives
                .Include(x => x.Creator)
                .Include(x => x.ArchiveOwner)
                .Include(x => x.SubSubjectClassification.SubjectClassification)
                    .ThenInclude(x => x.Classification)
                .Include(x => x.ArchiveType)
                .Include(x => x.TrxMediaStorageInActiveDetails)
                    .ThenInclude(x => x.MediaStorageInActive)
                .Include(y => y.TrxMediaStorageInActiveDetails)
                    .ThenInclude(x => x.MediaStorageInActive!.Row!.Level!.Rack!.Room)
                .AsNoTracking()
                .Where(x => x.IsActive == true && x.IsArchiveActive == true && x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActiveId != null)
                .Select(x => new ReportListArchiveInActive
                {
                    PemilikArsip = x.ArchiveOwner.ArchiveOwnerName,
                    AsalArsip = x.ArchiveOwner.ArchiveOwnerName,
                    UnitPencipta = x.Creator.CreatorName,
                    KodeKlasifikasi = x.SubSubjectClassification.SubSubjectClassificationCode,
                    NoArsip = x.DocumentNo,
                    SubjectArsip = x.SubSubjectClassification.SubjectClassification.SubjectClassificationName ?? string.Empty,
                    DeskripsiArsip = x.ArchiveDescription,
                    TahunArsip = x.CreatedDateArchive,
                    Jumlah = x.Volume,
                    Retensi = x.InactiveRetention,
                    Lokasi = x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.Room.RoomName
                    + " - " + x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RackName
                    + " - " + x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.LevelName
                    + " - " + x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.RowName,
                    KodeMediaSimpan = x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.MediaStorageInActiveCode,
                    JenisArsip = x.ArchiveType.ArchiveTypeName
                }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ReportListOfPurposeDestructionInActive>> GetReportListOfPurposeDestructionInActive()
        {
            var query = await _context.TrxArchives
                .Include(x => x.Creator)
                .Include(x => x.ArchiveOwner)
                .Include(x => x.SubSubjectClassification.SubjectClassification)
                    .ThenInclude(x => x.Classification)
                .Include(x => x.ArchiveType)
                .Include(x => x.TrxMediaStorageInActiveDetails)
                    .ThenInclude(x => x.MediaStorageInActive)
                .Include(y => y.TrxMediaStorageInActiveDetails)
                    .ThenInclude(x => x.MediaStorageInActive!.Row!.Level!.Rack!.Room)
                .Include(x => x.TrxArchiveDestroyDetails)
                    .ThenInclude(x => x.ArchiveDestroy)
                .AsNoTracking()
                .Where(x => x.IsActive == true && x.IsArchiveActive == true && x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActiveId != null && x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroyId != null)
                .Select(x => new ReportListOfPurposeDestructionInActive
                {
                    PemilikArsip = x.ArchiveOwner.ArchiveOwnerName,
                    AsalArsip = x.ArchiveOwner.ArchiveOwnerName,
                    UnitPencipta = x.Creator.CreatorName,
                    KodeKlasifikasi = x.SubSubjectClassification.SubSubjectClassificationCode,
                    NoArsip = x.DocumentNo,
                    SubjectArsip = x.SubSubjectClassification.SubjectClassification.SubjectClassificationName ?? string.Empty,
                    DeskripsiArsip = x.ArchiveDescription,
                    TahunArsip = x.CreatedDateArchive,
                    Jumlah = x.Volume,
                    Retensi = x.InactiveRetention,
                    Lokasi = x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.Room.RoomName
                    + " - " + x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RackName
                    + " - " + x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.LevelName
                    + " - " + x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.RowName,
                    KodeMediaSimpan = x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.MediaStorageInActiveCode,
                    JenisArsip = x.ArchiveType.ArchiveTypeName
                }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ReportTransferMediaArchiveInActive>> GetReportTransferMediaArchiveInActive()
        {
            var query = await _context.TrxArchives
                .Include(x => x.Creator)
                    .ThenInclude(x => x.ArchiveUnit.Company)
                .Include(x => x.ArchiveOwner)
                .Include(x => x.TrxFileArchiveDetails)
                .Include(x => x.SubSubjectClassification)
                    .ThenInclude(x => x.SubjectClassification)
                .Include(x => x.ArchiveType)
                .AsNoTracking()
                .Where(x => x.IsArchiveActive == true && x.IsActive == true && x.TrxFileArchiveDetails != null)
                .Select(x => new ReportTransferMediaArchiveInActive
                {
                    Perusahaan = x.Creator.ArchiveUnit.Company.CompanyName ?? string.Empty,
                    AsalArsip = x.ArchiveOwner.ArchiveOwnerName,
                    UnitPencipta = x.Creator.CreatorName,
                    Periode = x.CreatedDateArchive,
                    Jumlah = x.Volume,
                    TanggalPindai = x.CreatedDate,
                    KodeKlasifikasi = x.SubSubjectClassification.SubSubjectClassificationCode,
                    NoArsip = x.DocumentNo,
                    TipeArsip = x.ArchiveType.ArchiveTypeName
                }).ToListAsync();
                

            return query;
        }

        public async Task<IEnumerable<TransferMedia>> GetTransferMedias(ReportGlobalParams param)
        {
            var result = await _context.TrxArchives
              .Include(x => x.Creator.ArchiveUnit.Company)
              .Include(x => x.ArchiveOwner)
              .Include(x => x.TrxFileArchiveDetails)
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.ArchiveType)
              .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room)
              .Include(x => x.Gmd)
              .Include(x => x.ArchiveOwner)
              .Where(x => x.IsActive == true && x.IsArchiveActive == true)
              .Where(x => x.TrxFileArchiveDetails != null)
              .Where(x => (param.companyId == Guid.Empty ? true : x.Creator.ArchiveUnit.CompanyId == param.companyId))
              .Where(x => (param.archiveUnitId == Guid.Empty ? true : x.Creator.ArchiveUnitId == param.archiveUnitId))
              .Where(x => (param.roomId == Guid.Empty ? true : x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.Rack.RoomId == param.roomId))
              .Where(x => (param.rackId == Guid.Empty ? true : x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.RackId == param.rackId))
              .Where(x => (param.levelId == Guid.Empty ? true : x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.LevelId == param.levelId))
              .Where(x => (param.rowId == Guid.Empty ? true : x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.RowId == param.rowId))
              .Where(x => (param.gmdId == Guid.Empty ? true : x.GmdId == param.gmdId))
              .Where(x => (param.creatorId == Guid.Empty ? true : x.CreatorId == param.creatorId))
              .Where(x => (param.archiveOwnerId == Guid.Empty ? true : x.ArchiveOwnerId == param.archiveOwnerId))
              .Where(x => (param.classificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassification.ClassificationId == param.classificationId))
              .Where(x => (param.subjectClassificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassificationId == param.subjectClassificationId))
              .Where(x => (param.status == null ? true : x.IsUsed == param.status))
              .Where(x => (param.startDate == null ? true : x.CreatedDateArchive >= param.startDate))
              .Where(x => (param.endDate == null ? x.CreatedDateArchive <= DateTime.Now : x.CreatedDateArchive <= param.endDate))
              .Where(x => (param.startDateCreated == null ? true : x.CreatedDate >= param.startDateCreated))
              .Where(x => (param.endDateCreated == null ? x.CreatedDate <= DateTime.Now : x.CreatedDate <= param.endDateCreated))
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
