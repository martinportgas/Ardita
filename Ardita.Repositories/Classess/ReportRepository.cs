﻿using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ReportModels;
using Ardita.Models.ViewModels;
using Ardita.Report;
using Ardita.Repositories.Interfaces;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{

    public class ReportRepository : IReportRepository
    {
        private readonly BksArditaDevContext _context;

        public ReportRepository(BksArditaDevContext context) => _context = context;
        public async Task<string> GetGlobalParamsDescription(ReportGlobalParams param, string item)
        {
            await Task.Delay(0);
            string result = GlobalConst.SelectAll;
            if (item == GlobalConst.ParamNameCompany)
                result = param.companyId == Guid.Empty ? GlobalConst.SelectAll : _context.MstCompanies.FirstOrDefault(x => x.CompanyId == param.companyId).CompanyName;
            if (item == GlobalConst.ParamNameUnitArchive)
                result = param.archiveUnitId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxArchiveUnits.FirstOrDefault(x => x.ArchiveUnitId == param.archiveUnitId).ArchiveUnitName;
            if (item == GlobalConst.ParamNameUnitArchiveFrom)
                result = param.archiveUnitId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxArchiveUnits.FirstOrDefault(x => x.ArchiveUnitId == param.archiveUnitFromId).ArchiveUnitName;
            if (item == GlobalConst.ParamNameRoom)
                result = param.roomId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxRooms.FirstOrDefault(x => x.RoomId == param.roomId).RoomName;
            if (item == GlobalConst.ParamNameRack)
                result = param.rackId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxRacks.FirstOrDefault(x => x.RackId == param.rackId).RackName;
            if (item == GlobalConst.ParamNameLevel)
                result = param.levelId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxLevels.FirstOrDefault(x => x.LevelId == param.levelId).LevelName;
            if (item == GlobalConst.ParamNameRow)
                result = param.rowId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxRows.FirstOrDefault(x => x.RowId == param.rowId).RowName;
            if (item == GlobalConst.ParamNameStatus)
                result = param.status == null ? GlobalConst.SelectAll : param.status == true ? GlobalConst.Used : GlobalConst.Available;
            if (item == GlobalConst.ParamNameGmd)
                result = param.gmdId == Guid.Empty ? GlobalConst.SelectAll : _context.MstGmds.FirstOrDefault(x => x.GmdId == param.gmdId).GmdName;
            if (item == GlobalConst.ParamNameCreator)
                result = param.creatorId == Guid.Empty ? GlobalConst.SelectAll : _context.MstCreators.FirstOrDefault(x => x.CreatorId == param.creatorId).CreatorName;
            if (item == GlobalConst.ParamNameOwner)
                result = param.archiveOwnerId == Guid.Empty ? GlobalConst.SelectAll : _context.MstArchiveOwners.FirstOrDefault(x => x.ArchiveOwnerId == param.archiveOwnerId).ArchiveOwnerName;
            if (item == GlobalConst.ParamNameClassification)
                result = param.classificationId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxClassifications.FirstOrDefault(x => x.ClassificationId == param.classificationId).ClassificationName;
            if (item == GlobalConst.ParamNameSubjectClassification)
                result = param.subjectClassificationId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxSubjectClassifications.FirstOrDefault(x => x.SubjectClassificationId == param.subjectClassificationId).SubjectClassificationName;
            if (item == GlobalConst.ParamNameTypeStorage)
                result = param.typeStorageId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxTypeStorages.FirstOrDefault(x => x.TypeStorageId == param.typeStorageId).TypeStorageName;
            if (item == GlobalConst.ParamNameCreatePeriode)
                result = (param.startDate == null ? "-" : ((DateTime)param.startDate).ToString("dd-MM-yyyy")) + " s/d " + (param.endDate == null ? "-" : ((DateTime)param.endDate).ToString("dd-MM-yyyy"));
            if (item == GlobalConst.ParamNameInputPeriode)
                result = (param.startDateCreated == null ? "-" : ((DateTime)param.startDateCreated).ToString("dd-MM-yyyy")) + " s/d " + (param.endDateCreated == null ? "-" : ((DateTime)param.endDateCreated).ToString("dd-MM-yyyy"));
            if (item == GlobalConst.ParamNameDestroyPeriode)
                result = (param.startDateDestroy == null ? "-" : ((DateTime)param.startDateDestroy).ToString("dd-MM-yyyy")) + " s/d " + (param.endDateDestroy == null ? "-" : ((DateTime)param.endDateDestroy).ToString("dd-MM-yyyy"));
            if (item == GlobalConst.ParamNameRentPeriode)
                result = (param.startDateRent == null ? "-" : ((DateTime)param.startDateRent).ToString("dd-MM-yyyy")) + " s/d " + (param.endDateRent == null ? "-" : ((DateTime)param.endDateRent).ToString("dd-MM-yyyy"));
            if (item == GlobalConst.ParamNameMovementPeriode)
                result = (param.startDateMove == null ? "-" : ((DateTime)param.startDateMove).ToString("dd-MM-yyyy")) + " s/d " + (param.endDateMove == null ? "-" : ((DateTime)param.endDateMove).ToString("dd-MM-yyyy"));
            if (item == GlobalConst.ParamNameReceivePeriode)
                result = (param.startDateReceive == null ? "-" : ((DateTime)param.startDateReceive).ToString("dd-MM-yyyy")) + " s/d " + (param.endDateReceive == null ? "-" : ((DateTime)param.endDateReceive).ToString("dd-MM-yyyy"));
            if (item == GlobalConst.ParamNameBorrower)
                result = param.borrowerId == Guid.Empty ? GlobalConst.SelectAll : _context.MstBorrowers.FirstOrDefault(x => x.BorrowerId == param.borrowerId).BorrowerName;
            if (item == GlobalConst.ParamNamePIC)
                result = param.PIC == Guid.Empty ? GlobalConst.SelectAll : _context.MstEmployees.AsNoTracking().FirstOrDefault(x => x.EmployeeId == param.PIC).Name;
            if (item == GlobalConst.ParamNameSender)
                result = param.sender == Guid.Empty ? GlobalConst.SelectAll : _context.MstEmployees.AsNoTracking().FirstOrDefault(x => x.EmployeeId == param.sender).Name;
            if (item == GlobalConst.ParamNameReceiver)
                result = param.receiver == Guid.Empty ? GlobalConst.SelectAll : _context.MstEmployees.AsNoTracking().FirstOrDefault(x => x.EmployeeId == param.receiver).Name;

            return result;
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
            parameters.Add("typeStorage", param.typeStorageId == Guid.Empty ? GlobalConst.SelectAll : _context.TrxTypeStorages.FirstOrDefault(x => x.TypeStorageId == param.typeStorageId).TypeStorageName);
            parameters.Add("createPeriode", (param.startDate == null ? "-" : ((DateTime)param.startDate).ToString("dd-MM-yyyy")) + " s/d " + (param.endDate == null ? "-" : ((DateTime)param.endDate).ToString("dd-MM-yyyy")));
            parameters.Add("inputPeriode", (param.startDateCreated == null ? "-" : ((DateTime)param.startDateCreated).ToString("dd-MM-yyyy")) + " s/d " + (param.endDateCreated == null ? "-" : ((DateTime)param.endDateCreated).ToString("dd-MM-yyyy")));
            parameters.Add("destroyPeriode", (param.startDateDestroy == null ? "-" : ((DateTime)param.startDateDestroy).ToString("dd-MM-yyyy")) + " s/d " + (param.endDateDestroy == null ? "-" : ((DateTime)param.endDateDestroy).ToString("dd-MM-yyyy")));
            parameters.Add("rentPeriode", (param.startDateRent == null ? "-" : ((DateTime)param.startDateRent).ToString("dd-MM-yyyy")) + " s/d " + (param.endDateRent == null ? "-" : ((DateTime)param.endDateRent).ToString("dd-MM-yyyy")));
            return parameters;
        }

        public async Task<IEnumerable<ReportArchiveLoansInActive>> GetReportArchiveLoansInActive(ReportGlobalParams param, SessionModel User)
        {
            var query = await _context.TrxArchiveRentDetails
                .Include(x => x.TrxArchiveRent.TrxRentHistories)
                    .ThenInclude(x => x.Borrower)
                .Include(x => x.Archive.Creator)
                .Include(x => x.Archive.ArchiveOwner)
                .Include(x => x.Archive.ArchiveType)
                .Include(x => x.Archive.SubSubjectClassification.SubjectClassification.Classification)
                .AsNoTracking()
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.Archive.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Archive.Creator.ArchiveUnitId == User.ArchiveUnitId : x.Archive.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestination == User.ArchiveUnitId))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.Archive.CreatorId == User.CreatorId))
                .Where(x => (param.companyId == Guid.Empty ? true : x.Archive.Creator.ArchiveUnit.CompanyId == param.companyId))
                .Where(x => (param.archiveUnitId == Guid.Empty ? true : x.Archive.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Archive.Creator.ArchiveUnitId == param.archiveUnitId : x.Archive.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestination == param.archiveUnitId))
                .Where(x => (param.roomId == Guid.Empty ? true : x.Archive.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.Rack.RoomId == param.roomId))
                .Where(x => (param.rackId == Guid.Empty ? true : x.Archive.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.RackId == param.rackId))
                .Where(x => (param.levelId == Guid.Empty ? true : x.Archive.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.LevelId == param.levelId))
                .Where(x => (param.rowId == Guid.Empty ? true : x.Archive.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.RowId == param.rowId))
                .Where(x => (param.gmdId == Guid.Empty ? true : x.Archive.GmdId == param.gmdId))
                .Where(x => (param.typeStorageId == Guid.Empty ? true : x.Archive.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.TypeStorageId == param.typeStorageId))
                .Where(x => (param.creatorId == Guid.Empty ? true : x.Archive.CreatorId == param.creatorId))
                .Where(x => (param.borrowerId == Guid.Empty ? true : x.TrxArchiveRent.TrxRentHistories.FirstOrDefault().BorrowerId == param.borrowerId))
                .Where(x => (param.PIC == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.FirstOrDefault().CreatedBy == param.PIC))
                .Where(x => (param.archiveOwnerId == Guid.Empty ? true : x.Archive.ArchiveOwnerId == param.archiveOwnerId))
                .Where(x => (param.classificationId == Guid.Empty ? true : x.Archive.SubSubjectClassification.SubjectClassification.ClassificationId == param.classificationId))
                .Where(x => (param.subjectClassificationId == Guid.Empty ? true : x.Archive.SubSubjectClassification.SubjectClassificationId == param.subjectClassificationId))
                .Where(x => (param.statusId == 0 ? true : x.TrxArchiveRent.StatusId == param.statusId))
                .Where(x => (param.startDate == null ? true : x.Archive.CreatedDateArchive.Date >= param.startDate))
                .Where(x => (param.endDate == null ? true : x.Archive.CreatedDateArchive.Date <= param.endDate))
                .Where(x => (param.startDateRent == null ? true : (x.TrxArchiveRent.ApprovalDate == null ? ((DateTime)x.TrxArchiveRent.RequestedDate).Date : ((DateTime)x.TrxArchiveRent.ApprovalDate).Date) >= param.startDateRent))
                .Where(x => (param.endDateRent == null ? true : (x.TrxArchiveRent.ApprovalDate == null ? ((DateTime)x.TrxArchiveRent.RequestedDate).Date : (DateTime)x.TrxArchiveRent.ApprovalDate).Date <= param.endDateRent))
                .Select(x => new ReportArchiveLoansInActive
                {
                    PemilikArsip = x.Archive.ArchiveOwner.ArchiveOwnerName,
                    AsalArsip = x.Archive.TypeSender,
                    UnitPencipta = x.Archive.Creator.CreatorName,
                    Jumlah = x.Archive.Volume,
                    KodeKlasifikasi = x.Archive.SubSubjectClassification.SubjectClassification.Classification.ClassificationName,
                    NoArsip = x.Archive.DocumentNo,
                    JenisArsip = x.Archive.ArchiveType.ArchiveTypeName,
                    NamaPeminjam = x.TrxArchiveRent.TrxRentHistories.FirstOrDefault().Borrower.BorrowerName,
                    Perusahaan = x.TrxArchiveRent.TrxRentHistories.FirstOrDefault().Borrower.BorrowerCompany,
                    UnitKerja = x.TrxArchiveRent.TrxRentHistories.FirstOrDefault().Borrower.BorrowerArchiveUnit,
                    TanggalPinjam = x.TrxArchiveRent.ApprovalDate == null ? x.TrxArchiveRent.RequestedDate : x.TrxArchiveRent.ApprovalDate,
                    TanggalKembali = x.TrxArchiveRent.ApprovalReturnDate == null ? x.TrxArchiveRent.RequestedReturnDate : x.TrxArchiveRent.ApprovalReturnDate,
                    Period = x.Archive.CreatedDateArchive
                }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ReportArchiveProcessingInActive>> GetReportArchiveProcessingInActive(ReportGlobalParams param, SessionModel User)
        {
            var statusWaiting = (int)GlobalConst.STATUS.ArchiveNotReceived;
            var statusReceived = (int)GlobalConst.STATUS.ArchiveReceived;

            var query = await _context.TrxArchives
                .Include(x => x.Creator.ArchiveUnit)
                .Include(x => x.ArchiveOwner)
                .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
                .Include(x => x.ArchiveType)
                .Include(x => x.TrxMediaStorageInActiveDetails)
                    .ThenInclude(x => x.MediaStorageInActive)
                .Include(y => y.TrxArchiveMovementDetails)
                    .ThenInclude(y => y.ArchiveMovement.ArchiveUnitIdDestinationNavigation)
                .Include(y => y.TrxArchiveMovementDetails)
                    .ThenInclude(y => y.ArchiveMovement.ArchiveUnitIdFromNavigation)
                .AsNoTracking()
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnitId == User.ArchiveUnitId : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestination == User.ArchiveUnitId))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
                .Where(x => x.IsActive == true && x.IsArchiveActive == false)
                .Where(x => x.TrxMediaStorageInActiveDetails.FirstOrDefault() == null)
                .Where(x => (param.companyId == Guid.Empty ? true : x.Creator.ArchiveUnit.CompanyId == param.companyId))
                .Where(x => (param.archiveUnitFromId == Guid.Empty ? true : x.TrxArchiveMovementDetails == null ? x.Creator.ArchiveUnitId == param.archiveUnitFromId : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdFrom == param.archiveUnitFromId))
                .Where(x => (param.archiveUnitId == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnitId == param.archiveUnitId : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestination == param.archiveUnitId))
                .Where(x => (param.gmdId == Guid.Empty ? true : x.GmdId == param.gmdId))
                .Where(x => (param.archiveOwnerId == Guid.Empty ? true : x.ArchiveOwnerId == param.archiveOwnerId))
                .Where(x => (param.sender == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? false : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.CreatedByNavigation.EmployeeId == param.sender))
                .Where(x => (param.receiver == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.CreatedByNavigation.EmployeeId == param.receiver : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ReceivedByNavigation.EmployeeId == param.receiver))
                .Where(x => (param.creatorId == Guid.Empty ? true : x.CreatorId == param.creatorId))
                .Where(x => (param.classificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassification.ClassificationId == param.classificationId))
                .Where(x => (param.subjectClassificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassificationId == param.subjectClassificationId))
                //.Where(x => (param.statusId == 0 ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? param.statusId == statusWaiting ? false : param.statusId == statusReceived ? true : false : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.StatusReceived == param.statusId))
                .Where(x => (param.startDate == null ? true : x.CreatedDateArchive.Date >= param.startDate))
                .Where(x => (param.endDate == null ? true : x.CreatedDateArchive.Date <= param.endDate))
                .Where(x => (param.startDateReceive == null ? true : ((DateTime)x.InactiveDate).Date >= param.startDateReceive))
                .Where(x => (param.endDateReceive == null ? true : ((DateTime)x.InactiveDate).Date <= param.endDateReceive))
                .Where(x => (param.startDateCreated == null ? true : x.CreatedDate.Date >= param.startDateCreated))
                .Where(x => (param.endDateCreated == null ? true : x.CreatedDate.Date <= param.endDateCreated))
                .Select(x => new ReportArchiveProcessingInActive
                {
                    PemilikArsip = x.ArchiveOwner.ArchiveOwnerName,
                    AsalArsip = x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnit.ArchiveUnitName : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdFromNavigation.ArchiveUnitName,
                    UnitPencipta = x.Creator.CreatorName,
                    Jumlah = x.Volume,
                    TanggalDiterima = x.InactiveDate,
                    KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationName ?? string.Empty,
                    NoArsip = x.DocumentNo,
                    JenisArsip = x.ArchiveType.ArchiveTypeName,
                    TahunPenciptaan = x.CreatedDateArchive,
                    Retensi = x.InactiveRetention,
                    Lokasi = x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnit.ArchiveUnitName : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestinationNavigation.ArchiveUnitName,
                    PeriodeOlah = (DateTime)x.InactiveDate
                }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ReportArchiveReceivedInActive>> GetReportArchiveReceivedInActive(ReportGlobalParams param, SessionModel User)
        {
            var statusWaiting = (int)GlobalConst.STATUS.ArchiveNotReceived;
            var statusReceived = (int)GlobalConst.STATUS.ArchiveReceived;

            var query = await _context.TrxArchives
                .Include(x => x.Creator.ArchiveUnit)
                .Include(x => x.ArchiveOwner)
                .Include(x => x.Gmd)
                .Include(z => z.TrxArchiveMovementDetails).
                    ThenInclude(z => z.ArchiveMovement.CreatedByNavigation.Employee)
                .Include(p => p.TrxArchiveMovementDetails).
                    ThenInclude(p => p.ArchiveMovement.ReceivedByNavigation.Employee)
                .AsNoTracking()
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnitId == User.ArchiveUnitId : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestination == User.ArchiveUnitId))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
                .Where(x => x.IsActive == true)
                .Where(x => (x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.IsArchiveActive == false : true))
                .Where(x => (param.companyId == Guid.Empty ? true : x.Creator.ArchiveUnit.CompanyId == param.companyId))
                .Where(x => (param.archiveUnitFromId == Guid.Empty ? true : x.TrxArchiveMovementDetails == null ? x.Creator.ArchiveUnitId == param.archiveUnitFromId : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdFrom == param.archiveUnitFromId))
                .Where(x => (param.gmdId == Guid.Empty ? true : x.GmdId == param.gmdId))
                .Where(x => (param.archiveOwnerId == Guid.Empty ? true : x.ArchiveOwnerId == param.archiveOwnerId))
                .Where(x => (param.sender == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? false : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.CreatedByNavigation.EmployeeId == param.sender))
                .Where(x => (param.receiver == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.CreatedByNavigation.EmployeeId == param.receiver : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ReceivedByNavigation.EmployeeId == param.receiver))
                .Where(x => (param.statusId == 0 ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? param.statusId == statusWaiting ? false : param.statusId == statusReceived ? true : false : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.StatusReceived == param.statusId))
                .Where(x => (param.startDate == null ? true : x.CreatedDateArchive >= param.startDate))
                .Where(x => (param.endDate == null ? true : x.CreatedDateArchive <= param.endDate))
                .Where(x => (param.startDateReceive == null ? true : x.InactiveDate >= param.startDateReceive))
                .Where(x => (param.endDateReceive == null ? true : x.InactiveDate <= param.endDateReceive))
                .Select(x => new ReportArchiveReceivedInActive
                {
                    PemilikArsip = x.ArchiveOwner.ArchiveOwnerName,
                    AsalArsip = x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnit.ArchiveUnitName : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdFromNavigation.ArchiveUnitName,
                    Pengirim = x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.CreatedByNavigation.Employee.Name,
                    Penerima = x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ReceivedByNavigation.Employee.Name,
                    Tanggal = x.InactiveDate,
                    DokumenSerahTerima = x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.DocumentCode ?? string.Empty,
                    Jumlah = x.Volume,
                    NoArsip = x.DocumentNo
                }).ToListAsync();

            return query;
        }

        public async Task<IEnumerable<ArchiveActive>> GetArchiveActives(ReportGlobalParams param, SessionModel User)
        {
            var statusSubmit = (int)GlobalConst.STATUS.Submit;
            var result = await _context.TrxArchives
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room)
              .Include(x => x.Creator.ArchiveUnit.Company)
              .Include(x => x.Gmd)
              .Include(x => x.ArchiveOwner)
              .AsNoTracking()
              .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.Creator.ArchiveUnitId == User.ArchiveUnitId))
              .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
              .Where(x => x.IsActive == true && x.IsArchiveActive == true)
              //.Where(x => x.StatusId == statusSubmit)
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
              .Where(x => (param.endDate == null ? true : x.CreatedDateArchive <= param.endDate))
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

        public async Task<IEnumerable<ArchiveDestroy>> GetArchiveDestroys(ReportGlobalParams param, SessionModel User)
        {
            var result = await _context.TrxArchives
              .Include(x => x.Creator.ArchiveUnit.Company)
              .Include(x => x.ArchiveOwner)
              .Include(x => x.ArchiveType)
              .Include(x => x.Gmd)
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.ArchiveDestroy)
              .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room)
              .AsNoTracking()
              .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.Creator.ArchiveUnitId == User.ArchiveUnitId))
              .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
              .Where(x => x.TrxArchiveDestroyDetails.FirstOrDefault() != null && x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.IsArchiveActive == true)
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
              .Where(x => (param.statusId == 0 ? true : x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.StatusId == param.statusId))
              .Where(x => (param.startDate == null ? true : x.CreatedDateArchive.Date >= param.startDate))
              .Where(x => (param.endDate == null ? true : x.CreatedDateArchive <= param.endDate))
              .Where(x => (param.startDateCreated == null ? true : x.CreatedDate.Date >= param.startDate))
              .Where(x => (param.endDateCreated == null ? true : x.CreatedDate <= param.endDate))
              .Where(x => (param.startDateDestroy == null ? true : x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.DestroySchedule.Date >= param.startDate))
              .Where(x => (param.endDateDestroy == null ? true : x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.DestroySchedule.Date <= param.endDate))
              .Select(x => new ArchiveDestroy
              {
                  Perusahaan = x.Creator.ArchiveUnit.Company.CompanyName,
                  AsalArsip = x.ArchiveOwner.ArchiveOwnerName,
                  PenciptaArsip = x.Creator.CreatorName,
                  Period = x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.CreatedDate,
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
        public async Task<IEnumerable<ArchiveDestroy>> GetArchiveInActiveDestroys(ReportGlobalParams param, SessionModel User)
        {
            var result = await _context.TrxArchives
              .Include(x => x.Creator.ArchiveUnit.Company)
              .Include(x => x.ArchiveOwner)
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.ArchiveType)
              .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.ArchiveDestroy)
              .Include(x => x.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room)
              .Include(p => p.TrxArchiveMovementDetails).ThenInclude(p => p.ArchiveMovement.ArchiveUnitIdDestinationNavigation)
              .AsNoTracking()
              .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnitId == User.ArchiveUnitId : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestination == User.ArchiveUnitId))
              .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
              .Where(x => x.TrxArchiveDestroyDetails.FirstOrDefault() != null && x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.IsArchiveActive == false)
              .Where(x => (param.companyId == Guid.Empty ? true : x.Creator.ArchiveUnit.CompanyId == param.companyId))
              .Where(x => (param.archiveUnitId == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnitId == param.archiveUnitId : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestination == param.archiveUnitId))
              .Where(x => (param.roomId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RoomId == param.roomId))
              .Where(x => (param.rackId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.RackId == param.rackId))
              .Where(x => (param.levelId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.LevelId == param.levelId))
              .Where(x => (param.rowId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.RowId == param.rowId))
              .Where(x => (param.gmdId == Guid.Empty ? true : x.GmdId == param.gmdId))
              .Where(x => (param.creatorId == Guid.Empty ? true : x.CreatorId == param.creatorId))
              .Where(x => (param.archiveOwnerId == Guid.Empty ? true : x.ArchiveOwnerId == param.archiveOwnerId))
              .Where(x => (param.classificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassification.ClassificationId == param.classificationId))
              .Where(x => (param.subjectClassificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassificationId == param.subjectClassificationId))
              .Where(x => (param.statusId == 0 ? true : x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.StatusId == param.statusId))
              .Where(x => (param.startDate == null ? true : x.CreatedDateArchive.Date >= param.startDate))
              .Where(x => (param.endDate == null ? true : x.CreatedDateArchive.Date <= param.endDate))
              .Where(x => (param.startDateCreated == null ? true : x.CreatedDate.Date >= param.startDate))
              .Where(x => (param.endDateCreated == null ? true : x.CreatedDate.Date <= param.endDate))
              .Where(x => (param.startDateDestroy == null ? true : x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.CreatedDate.Date >= param.startDate))
              .Where(x => (param.endDateDestroy == null ? true : x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.CreatedDate.Date <= param.endDate))
              .Select(x => new ArchiveDestroy
              {
                  Perusahaan = x.Creator.ArchiveUnit.Company.CompanyName,
                  AsalArsip = x.ArchiveOwner.ArchiveOwnerName,
                  PenciptaArsip = x.Creator.CreatorName,
                  Period = x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.DestroySchedule,
                  Jumlah = x.Volume.ToString(),
                  SifatArsip = x.Description,
                  KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationCode,
                  NomorArsip = x.DocumentNo,
                  TipeArsip = x.ArchiveType.ArchiveTypeName,
                  RetensiInAktif = x.InactiveRetention.ToString(),
                  KodeMediaSimpan = x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.MediaStorageInActiveCode
              })
              .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<ArchiveMovement>> GetArchiveMovements(ReportGlobalParams param, SessionModel User)
        {
            var result = await _context.TrxArchives
              .Include(x => x.Creator.ArchiveUnit.Company)
              .Include(x => x.ArchiveOwner)
              .Include(x => x.Gmd)
              .Include(x => x.ArchiveType)
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.ArchiveMovement)
              .Include(x => x.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage)
              .AsNoTracking()
              .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.Creator.ArchiveUnitId == User.ArchiveUnitId))
              .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
              .Where(x => x.TrxArchiveMovementDetails.FirstOrDefault() != null)
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
              .Where(x => (param.statusId == 0 ? true : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.StatusId == param.statusId))
              .Where(x => (param.startDate == null ? true : x.CreatedDateArchive.Date >= param.startDate))
              .Where(x => (param.endDate == null ? true: x.CreatedDateArchive.Date <= param.endDate))
              .Where(x => (param.startDateCreated == null ? true : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.CreatedDate.Date >= param.startDateCreated))
              .Where(x => (param.endDateCreated == null ? true : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.CreatedDate.Date <= param.endDateCreated))
              .Where(x => (param.startDateMove == null ? true : ((DateTime)x.InactiveDate).Date >= param.startDateMove))
              .Where(x => (param.endDateMove == null ? true : ((DateTime)x.InactiveDate).Date <= param.endDateMove))
              .Select(x => new ArchiveMovement
              {
                  Perusahaan = x.Creator.ArchiveUnit.Company.CompanyName,
                  AsalArsip = x.ArchiveOwner.ArchiveOwnerName,
                  PenciptaArsip = x.Creator.CreatorName,
                  Period = x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.CreatedDate,
                  Jumlah = x.Volume.ToString(),
                  TanggalPindah = (DateTime)x.InactiveDate,
                  KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationCode,
                  NomorArsip = x.DocumentNo,
                  TipeArsip = x.ArchiveType.ArchiveTypeName,
                  RetensiInAktif = x.ActiveRetention.ToString(),
                  KodeMediaSimpan = x.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.MediaStorageCode

              })
              .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<ArchiveUsed>> GetArchiveUseds(ReportGlobalParams param, SessionModel User)
        {
            var result = await _context.TrxArchiveOutIndicators
            .Include(x => x.Archive.SubSubjectClassification.SubjectClassification.Classification)
            .Include(x => x.Archive.Creator.ArchiveUnit)
            .Include(x => x.Archive.Gmd)
            .Include(x => x.Archive.ArchiveOwner)
            .Include(x => x.Archive.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage)
            .AsNoTracking()
            .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.Archive.Creator.ArchiveUnitId == User.ArchiveUnitId))
            .Where(x => (User.CreatorId == Guid.Empty ? true : x.Archive.CreatorId == User.CreatorId))
            .Where(x => (param.companyId == Guid.Empty ? true : x.Archive.Creator.ArchiveUnit.CompanyId == param.companyId))
            .Where(x => (param.archiveUnitId == Guid.Empty ? true : x.Archive.Creator.ArchiveUnitId == param.archiveUnitId))
            .Where(x => (param.roomId == Guid.Empty ? true : x.Archive.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.Rack.RoomId == param.roomId))
            .Where(x => (param.rackId == Guid.Empty ? true : x.Archive.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.RackId == param.rackId))
            .Where(x => (param.levelId == Guid.Empty ? true : x.Archive.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.LevelId == param.levelId))
            .Where(x => (param.rowId == Guid.Empty ? true : x.Archive.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.RowId == param.rowId))
            .Where(x => (param.gmdId == Guid.Empty ? true : x.Archive.GmdId == param.gmdId))
            .Where(x => (param.creatorId == Guid.Empty ? true : x.Archive.CreatorId == param.creatorId))
            .Where(x => (param.archiveOwnerId == Guid.Empty ? true : x.Archive.ArchiveOwnerId == param.archiveOwnerId))
            .Where(x => (param.classificationId == Guid.Empty ? true : x.Archive.SubSubjectClassification.SubjectClassification.ClassificationId == param.classificationId))
            .Where(x => (param.subjectClassificationId == Guid.Empty ? true : x.Archive.SubSubjectClassification.SubjectClassificationId == param.subjectClassificationId))
            .Where(x => (param.status == null ? true : (bool)param.status ? x.ReturnDate == null : x.ReturnDate != null))
            .Where(x => (param.startDate == null ? true : x.Archive.CreatedDateArchive.Date >= param.startDate))
            .Where(x => (param.endDate == null ? true: x.Archive.CreatedDateArchive.Date <= param.endDate))
            .Where(x => (param.startDateCreated == null ? true : x.Archive.CreatedDate >= param.startDate))
            .Where(x => (param.endDateCreated == null ? true : x.Archive.CreatedDate <= param.endDate))
            .Where(x => (param.startDateUse == null ? true : x.UsedDate >= param.startDateUse))
            .Where(x => (param.endDateUse == null ? true : x.UsedDate <= param.endDateUse))
            .Select(x => new ArchiveUsed
            {
                NoDocumen = x.Archive.DocumentNo,
                NoItemArsip = x.Archive.ArchiveCode,
                KodeKlasifikasi = x.Archive.SubSubjectClassification.SubjectClassification.Classification.ClassificationCode,
                JudulArsip = x.Archive.TitleArchive,
                UraianInformasiArsip = x.Archive.ArchiveDescription,
                Tanggal = x.UsedDate,
                Jumlah = x.Archive.Volume.ToString(),
                KodeMediaSimpan = x.Archive.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.MediaStorageCode
            })
            .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<ReportDocument>> GetReportDocument(ReportGlobalParams param, SessionModel User)
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

        public async Task<IEnumerable<ReportListArchiveInActive>> GetReportListArchiveInActive(ReportGlobalParams param, SessionModel User)
        {
            var statusSubmit = (int)GlobalConst.STATUS.Submit;
            var result = await _context.TrxArchives
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room)
              .Include(x => x.Creator.ArchiveUnit.Company)
              .Include(x => x.Gmd)
              .Include(x => x.ArchiveOwner)
              .Include(y => y.TrxArchiveMovementDetails).ThenInclude(y => y.ArchiveMovement.ArchiveUnitIdFromNavigation)
              .AsNoTracking()
              .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnitId == User.ArchiveUnitId : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestination == User.ArchiveUnitId))
              .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
              .Where(x => x.IsActive == true && x.IsArchiveActive == false)
              .Where(x => x.StatusId == statusSubmit)
              .Where(x => x.SubSubjectClassification != null)
              .Where(x => (param.companyId == Guid.Empty ? true : x.Creator.ArchiveUnit.CompanyId == param.companyId))
              .Where(x => (param.archiveUnitId == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnitId == param.archiveUnitId : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestination == param.archiveUnitId))
              .Where(x => (param.roomId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RoomId == param.roomId))
              .Where(x => (param.rackId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.RackId == param.rackId))
              .Where(x => (param.levelId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.LevelId == param.levelId))
              .Where(x => (param.rowId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.RowId == param.rowId))
              .Where(x => (param.gmdId == Guid.Empty ? true : x.GmdId == param.gmdId))
              .Where(x => (param.typeStorageId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.TypeStorageId == param.typeStorageId))
              .Where(x => (param.creatorId == Guid.Empty ? true : x.CreatorId == param.creatorId))
              .Where(x => (param.archiveOwnerId == Guid.Empty ? true : x.ArchiveOwnerId == param.archiveOwnerId))
              .Where(x => (param.classificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassification.ClassificationId == param.classificationId))
              .Where(x => (param.subjectClassificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassificationId == param.subjectClassificationId))
              .Where(x => (param.status == null ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().IsRent == param.status))
              .Where(x => (param.startDate == null ? true : x.CreatedDateArchive.Date >= param.startDate))
              .Where(x => (param.endDate == null ? true : x.CreatedDateArchive.Date <= param.endDate))
              .Select(x => new ReportListArchiveInActive
              {
                    PemilikArsip = x.Creator.ArchiveUnit.Company.CompanyName,
                    AsalArsip = x.ArchiveOwner.ArchiveOwnerName,
                    UnitPencipta = x.Creator.CreatorName,
                    KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationName,
                    NoArsip = x.DocumentNo,
                    JenisArsip = x.ArchiveType.ArchiveTypeName,
                    SubjectArsip = x.SubSubjectClassification.SubjectClassification.SubjectClassificationName,
                    DeskripsiArsip = x.Description,
                    TahunArsip = x.CreatedDateArchive,
                    Jumlah = x.Volume,
                    Retensi = x.InactiveRetention,
                    Lokasi = x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnit.ArchiveUnitName : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestinationNavigation.ArchiveUnitName,
                    KodeMediaSimpan = x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.MediaStorageInActiveCode
                })
              .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<ReportListOfPurposeDestructionInActive>> GetReportListOfPurposeDestructionInActive(ReportGlobalParams param, SessionModel User)
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

        public async Task<IEnumerable<ReportTransferMediaArchiveInActive>> GetReportTransferMediaArchiveInActive(ReportGlobalParams param, SessionModel User)
        {
            var result = await _context.TrxArchives
              .Include(x => x.Creator.ArchiveUnit.Company)
              .Include(x => x.ArchiveOwner)
              .Include(x => x.TrxFileArchiveDetails)
              .Include(x => x.ArchiveType)
              .Include(x => x.Gmd)
              .Include(x => x.ArchiveOwner)
              .Include(x => x.SubSubjectClassification.SubjectClassification.Classification)
              .Include(x => x.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room)
              .Include(p => p.TrxArchiveMovementDetails).ThenInclude(p => p.ArchiveMovement.ArchiveUnitIdDestinationNavigation)
              .AsNoTracking()
              .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnitId == User.ArchiveUnitId : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestination == User.ArchiveUnitId))
              .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
              .Where(x => x.IsActive == true && x.IsArchiveActive == false)
              .Where(x => x.TrxFileArchiveDetails.FirstOrDefault() != null)
              .Where(x => (param.companyId == Guid.Empty ? true : x.Creator.ArchiveUnit.CompanyId == param.companyId))
              .Where(x => (param.archiveUnitId == Guid.Empty ? true : x.TrxArchiveMovementDetails.FirstOrDefault() == null ? x.Creator.ArchiveUnitId == param.archiveUnitId : x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.ArchiveUnitIdDestination == param.archiveUnitId))
              .Where(x => (param.roomId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RoomId == param.roomId))
              .Where(x => (param.rackId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.RackId == param.rackId))
              .Where(x => (param.levelId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.LevelId == param.levelId))
              .Where(x => (param.rowId == Guid.Empty ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.RowId == param.rowId))
              .Where(x => (param.gmdId == Guid.Empty ? true : x.GmdId == param.gmdId))
              .Where(x => (param.creatorId == Guid.Empty ? true : x.CreatorId == param.creatorId))
              .Where(x => (param.archiveOwnerId == Guid.Empty ? true : x.ArchiveOwnerId == param.archiveOwnerId))
              .Where(x => (param.classificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassification.ClassificationId == param.classificationId))
              .Where(x => (param.subjectClassificationId == Guid.Empty ? true : x.SubSubjectClassification.SubjectClassificationId == param.subjectClassificationId))
              .Where(x => (param.status == null ? true : x.TrxMediaStorageInActiveDetails.FirstOrDefault().IsRent == param.status))
              .Where(x => (param.startDate == null ? true : x.CreatedDateArchive.Date >= param.startDate))
              .Where(x => (param.endDate == null ? true : x.CreatedDateArchive.Date <= param.endDate))
              .Where(x => (param.startDateCreated == null ? true : x.CreatedDate.Date >= param.startDateCreated))
              .Where(x => (param.endDateCreated == null ? true : x.CreatedDate.Date <= param.endDateCreated))
              .Select(x => new ReportTransferMediaArchiveInActive
              {
                  Perusahaan = x.Creator.ArchiveUnit.Company.CompanyName,
                  AsalArsip = x.ArchiveOwner.ArchiveOwnerName,
                  UnitPencipta = x.Creator.CreatorName,
                  TanggalPindai = x.TrxFileArchiveDetails.FirstOrDefault().CreatedDate,
                  Periode = x.CreatedDateArchive,
                  Jumlah = x.Volume,
                  KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationName,
                  NoArsip = x.DocumentNo,
                  TipeArsip = x.ArchiveType.ArchiveTypeName
              })
              .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<TransferMedia>> GetTransferMedias(ReportGlobalParams param, SessionModel User)
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
              .AsNoTracking()
              .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.Creator.ArchiveUnitId == User.ArchiveUnitId))
              .Where(x => (User.CreatorId == Guid.Empty ? true : x.CreatorId == User.CreatorId))
              .Where(x => x.IsActive == true && x.IsArchiveActive == true)
              .Where(x => x.TrxFileArchiveDetails.FirstOrDefault() != null)
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
                  PeriodPindai = x.TrxFileArchiveDetails.FirstOrDefault().CreatedDate,
                  KodeKlasifikasi = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationCode,
                  NomorArsip = x.DocumentNo,
                  TipeArsip = x.ArchiveType.ArchiveTypeName
              })
              .ToListAsync();
            return result;
        }
    }
}
