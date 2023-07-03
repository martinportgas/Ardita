﻿using Ardita.Extensions;
using Ardita.Models.DbModels;
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

        public Task<IEnumerable<ReportArchiveLoansInActive>> GetReportArchiveLoansInActive()
        {
            throw new NotImplementedException();
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
            await Task.Delay(0);

            var query = _context.TrxArchives
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
                }).ToList()
                .GroupBy( x => new {
                    x.Perusahaan, 
                    x.AsalArsip,
                    x.UnitPencipta,
                    x.Periode,
                    x.Jumlah,
                    x.TanggalPindai,
                    x.KodeKlasifikasi,
                    x.NoArsip,
                    x.TipeArsip
                });

            return (IEnumerable<ReportTransferMediaArchiveInActive>)query;
        }
    }
}