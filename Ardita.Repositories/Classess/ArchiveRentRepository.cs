using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Report;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess
{
    public class ArchiveRentRepository : IArchiveRentRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public ArchiveRentRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }

        public async Task<int> Approval(Guid id, string description, int status, Guid User)
        {
            int result = 0;
            var data = await _context.TrxArchiveRents.FirstOrDefaultAsync(x => x.TrxArchiveRentId == id);
            if (data != null)
            {
                data.StatusId = status;
                if (status == 3)
                {
                    data.ApprovalDate = DateTime.Now;
                    data.ApprovalReturnDate = DateTime.Now.AddDays(7);
                    data.ApprovedBy = User;
                    data.StatusId = (int)GlobalConst.STATUS.WaitingForRetrieval;
                }
                else
                {
                    data.StatusId = (int)GlobalConst.STATUS.Rejected;
                    data.RejectedBy = User;
                }
               
                data.ApprovalNotes = description;
                data.UpdatedBy = User;
                data.UpdatedDate = DateTime.Now;

                _context.TrxArchiveRents.Update(data);
                result = await _context.SaveChangesAsync();

                if (status == (int)GlobalConst.STATUS.Approved)
                {
                    var dataStorage = await _context.TrxMediaStorageInActiveDetails.FirstOrDefaultAsync(x => x.ArchiveId == data.ArchiveId);
                    if (dataStorage != null)
                    {
                        var dataSubStorage = await _context.TrxMediaStorageInActiveDetails.Where(x => x.Sort == dataStorage.Sort && x.MediaStorageInActiveId == dataStorage.MediaStorageInActiveId).ToListAsync();
                        if (dataSubStorage.Any())
                        {
                            foreach (TrxMediaStorageInActiveDetail item in dataSubStorage)
                            {
                                item.IsRent = true;
                                _context.TrxMediaStorageInActiveDetails.Update(item);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }

            return result;
        }

        public Task<int> Delete(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TrxArchiveRent>> GetAll(string par = " 1=1 ")
        {
            return await _context.TrxArchiveRents
                .Where(par).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<object>> GetByBorrowerId(Guid Id)
        {
            var result = await _context.TrxRentHistories
                .Include(x => x.Borrower)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.Creator)
                .Include(x => x.TrxArchiveRent.Status)
                .Where(x => x.Borrower.BorrowerId == Id)
                .Select(x => new
                {
                    x.Borrower.BorrowerName,
                    x.Borrower.BorrowerCompany,
                    x.Borrower.BorrowerArchiveUnit,
                    x.Borrower.BorrowerPosition,
                    x.Borrower.BorrowerIdentityNumber,
                    x.Borrower.BorrowerPhone,
                    x.Borrower.BorrowerEmail,
                    x.TrxArchiveRentId,
                    ArchiveCode = x.TrxArchiveRent.TrxArchiveRentDetails.FirstOrDefault().Archive.ArchiveCode,
                    ArchiveTitle = x.TrxArchiveRent.TrxArchiveRentDetails.FirstOrDefault().Archive.TitleArchive,
                    CreatorName = x.TrxArchiveRent.TrxArchiveRentDetails.FirstOrDefault().Archive.Creator.CreatorName,
                    RentDate = x.TrxArchiveRent.RequestedDate,
                    ReturnDate = x.TrxArchiveRent.ReturnDate,
                    x.TrxArchiveRent.Description,
                    Status = x.TrxArchiveRent.Status.Name,
                    x.TrxArchiveRent.ApprovalNotes
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<object>> GetApprovalByFilterModel(DataTableModel model)
        {
            var User = AppUsers.CurrentUser(model.SessionUser);
            var result = await _context.TrxRentHistories
                 .Include(x => x.Borrower)
                 .Include(x => x.TrxArchiveRent.Status)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.Creator.ArchiveUnit)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                 .Where($"(Borrower.BorrowerName).Contains(@0)", model.searchValue)
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.Creator.ArchiveUnitId == User.ArchiveUnitId)))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                .Where(model.advanceSearch!.Search)
                 .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                 .Skip(model.skip).Take(model.pageSize)
                 .Select(x => new
                 {
                     x.TrxArchiveRentId,
                     Name = x.Borrower.BorrowerName,
                     Company = x.Borrower.BorrowerCompany,
                     UserCreatedBy = GetUserNameCreatedById(x.CreatedBy),
                     x.TrxArchiveRent.RequestedDate,
                     x.TrxArchiveRent.StatusId,
                     Status = x.TrxArchiveRent.Status.Name,
                     Color = x.TrxArchiveRent.Status.Color,
                 })
                 .ToListAsync();

            return result;
        }

        public async Task<int> GetApprovalCountByFilterModel(DataTableModel model)
        {
            var User = AppUsers.CurrentUser(model.SessionUser);
            var result = await _context.TrxRentHistories
                 .Include(x => x.Borrower)
                 .Include(x => x.TrxArchiveRent.Status)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.Creator.ArchiveUnit)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                 .Where($"(Borrower.BorrowerName).Contains(@0)", model.searchValue)
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.Creator.ArchiveUnitId == User.ArchiveUnitId)))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                .Where(model.advanceSearch!.Search)
               .CountAsync();

            return result;
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var User = AppUsers.CurrentUser(model.SessionUser);
            var result = await _context.TrxRentHistories
                .Include(x => x.Borrower)
                .Include(x => x.TrxArchiveRent.Status)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.Creator.ArchiveUnit)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                .Where($"(TrxArchiveRent.RequestedReturnDate.ToString()).Contains(@0)", model.searchValue)
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.Creator.ArchiveUnitId == User.ArchiveUnitId)))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                .Where(model.advanceSearch!.Search)
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new
                {
                    x.TrxArchiveRentId,
                    Name = x.Borrower.BorrowerName,
                    x.TrxArchiveRent.RequestedDate,
                    x.TrxArchiveRent.RequestedReturnDate,
                    x.TrxArchiveRent.StatusId,
                    Status = x.TrxArchiveRent.Status.Name,
                    Color = x.TrxArchiveRent.Status.Color,
                    UserCreatedBy = GetUserNameCreatedById(x.CreatedBy),
                })
                .ToListAsync();

            return result;
        }

        public async Task<TrxArchiveRent> GetById(Guid id)
        {
            var result = await _context.TrxArchiveRents
                .Include(x => x.Status)
                .Include(x => x.TrxRentHistories).ThenInclude(x => x.Borrower)
                .Include(x => x.TrxArchiveRentDetails).ThenInclude(x => x.MediaStorageInActive.TypeStorage.ArchiveUnit.Company)
                .Include(x => x.TrxArchiveRentDetails).ThenInclude(x => x.MediaStorageInActive.SubSubjectClassification.SubjectClassification.Classification)
                .Include(x => x.ApprovedByNavigation.Employee.Position)
                .Include(x => x.ApprovedByNavigation.Creator)
                .Include(x => x.TrxArchiveRentDetails).ThenInclude(x => x.Archive.Creator)
                .Include(x => x.TrxArchiveRentDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.SubTypeStorage)
                .Include(x => x.ApprovedByNavigation.Employee.Company)
                .Where(x => x.TrxArchiveRentId == id).ToListAsync();
            return result.FirstOrDefault();
        }
        public async Task<int> Insert(TrxArchiveRent model, MstBorrower borrower, List<TrxArchiveRentDetail> listDetail)
        {
            int result = 0;

            if (model != null)
            {
                var detail = await _context.TrxMediaStorageInActiveDetails
                .Include(x => x.MediaStorageInActive.TypeStorage.ArchiveUnit)
                .FirstOrDefaultAsync(x => x.ArchiveId == model.ArchiveId);

                var countData = await _context.TrxArchiveRents.CountAsync();
                int i = 1;
                var validRentCode = string.Empty;
                bool inValid = true;
                while (inValid)
                {
                    validRentCode = $"BA.{(countData + i).ToString("D3")}/{DateTime.Now.Month.ToString("D2")}/{DateTime.Now.Year}";
                    int count = await _context.TrxArchiveRents.Where(x => x.RentCode == validRentCode).CountAsync();
                    if (count > 0)
                        i++;
                    else
                        inValid = false;
                }

                model.RentCode = validRentCode;
                model.StatusId = (int)GlobalConst.STATUS.ApprovalProcess;

                _context.TrxArchiveRents.Add(model);
                result += await _context.SaveChangesAsync();

                _context.TrxArchiveRentDetails.AddRange(listDetail);
                result += await _context.SaveChangesAsync();

                _context.MstBorrowers.Add(borrower);
                result += await _context.SaveChangesAsync();

                var history = new TrxRentHistory();
                history.RentHistoryId = new Guid();
                history.TrxArchiveRentId = model.TrxArchiveRentId;
                history.BorrowerId = borrower.BorrowerId;
                history.CreatedBy = model.CreatedBy;
                history.CreatedDate = DateTime.Now;

                _context.TrxRentHistories.Add(history);
                result += await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxArchiveRent>(GlobalConst.New, (Guid)model.CreatedBy!, new List<TrxArchiveRent> { }, new List<TrxArchiveRent> { model });
                        await _logChangesRepository.CreateLog<MstBorrower>(GlobalConst.New, (Guid)model.CreatedBy!, new List<MstBorrower> { }, new List<MstBorrower> { borrower });
                        await _logChangesRepository.CreateLog<TrxArchiveRentDetail>(GlobalConst.New, (Guid)model.CreatedBy!, new List<TrxArchiveRentDetail> {  }, listDetail);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<int> Update(TrxArchiveRent model, MstBorrower borrower, List<TrxArchiveRentDetail> listDetail)
        {
            int result = 0;

            if (model != null)
            {

                var borrowerResult = await _context.MstBorrowers.AsNoTracking().FirstOrDefaultAsync(x => x.BorrowerId == borrower.BorrowerId);

                if (borrowerResult != null) {

                    var detail = await _context.TrxMediaStorageInActiveDetails
                    .Include(x => x.MediaStorageInActive.TypeStorage.ArchiveUnit)
                    .FirstOrDefaultAsync(x => x.ArchiveId == model.ArchiveId);

                    var countData = await _context.TrxArchiveRents.CountAsync();
                    int i = 1;
                    var validRentCode = string.Empty;
                    bool inValid = true;
                    while (inValid)
                    {
                        validRentCode = $"BA.{(countData + i).ToString("D3")}/{DateTime.Now.Month.ToString("D2")}/{DateTime.Now.Year}";
                        int count = await _context.TrxArchiveRents.Where(x => x.RentCode == validRentCode).CountAsync();
                        if (count > 0)
                            i++;
                        else
                            inValid = false;
                    }

                    model.RentCode = validRentCode;
                    model.StatusId = (int)GlobalConst.STATUS.ApprovalProcess;

                    _context.TrxArchiveRents.Add(model);
                    await _context.SaveChangesAsync();

                    _context.TrxArchiveRentDetails.AddRange(listDetail);
                    await _context.SaveChangesAsync();

                    borrower.BorrowerName = borrowerResult.BorrowerName;
                    borrower.CreatedDate = borrowerResult.CreatedDate;
                    borrower.CreatedBy = borrowerResult.CreatedBy;

                    _context.MstBorrowers.Update(borrower);
                    await _context.SaveChangesAsync();

                    var history = new TrxRentHistory();

                    history.RentHistoryId = new Guid();
                    history.TrxArchiveRentId = model.TrxArchiveRentId;
                    history.BorrowerId = borrower.BorrowerId;
                    history.CreatedBy = model.CreatedBy;
                    history.CreatedDate = DateTime.Now;

                    _context.TrxRentHistories.Add(history);
                    await _context.SaveChangesAsync();
                }

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxArchiveRent>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<TrxArchiveRent> { }, new List<TrxArchiveRent> { model });
                        await _logChangesRepository.CreateLog<MstBorrower>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<MstBorrower> { borrowerResult }, new List<MstBorrower> { borrower });
                        await _logChangesRepository.CreateLog<TrxArchiveRentDetail>(GlobalConst.Update, (Guid)model.UpdatedBy!, listDetail, listDetail);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var User = AppUsers.CurrentUser(model.SessionUser);
            var result = await _context.TrxRentHistories
                .Include(x => x.Borrower)
                .Include(x => x.TrxArchiveRent.Status)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.Creator.ArchiveUnit)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                .Where($"(TrxArchiveRent.RequestedReturnDate.ToString()).Contains(@0)", model.searchValue)
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.Creator.ArchiveUnitId == User.ArchiveUnitId)))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                .Where(model.advanceSearch!.Search)
               .CountAsync();

            return result;
        }

        #region Retrieval
        public async Task<IEnumerable<object>> GetRetrievalByFilterModel(DataTableModel model)
        {
            var User = AppUsers.CurrentUser(model.SessionUser);
            var result = await _context.TrxRentHistories
                 .Include(x => x.Borrower)
                 .Include(x => x.TrxArchiveRent.Status)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.Creator.ArchiveUnit)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                 .Where(x => x.TrxArchiveRent.RetrievalDate != null)
                 .Where($"(Borrower.BorrowerName).Contains(@0)", model.searchValue)
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.Creator.ArchiveUnitId == User.ArchiveUnitId)))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                .Where(model.advanceSearch!.Search)
                 .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                 .Skip(model.skip).Take(model.pageSize)
                 .Select(x => new
                 {
                     x.TrxArchiveRentId,
                     Name = x.Borrower.BorrowerName,
                     Company = x.Borrower.BorrowerCompany,
                     UserCreatedBy = GetUserNameCreatedById(x.CreatedBy),
                     x.TrxArchiveRent.ApprovalDate,
                     x.TrxArchiveRent.ApprovalReturnDate,
                     x.TrxArchiveRent.RetrievalDate,
                     Status = x.TrxArchiveRent.Status.Name,
                     Color = x.TrxArchiveRent.Status.Color,
                 })
                 .ToListAsync();

            return result;
        }

        public async Task<int> GetRetrievalCountByFilterModel(DataTableModel model)
        {
            var User = AppUsers.CurrentUser(model.SessionUser);
            var result = await _context.TrxRentHistories
                 .Include(x => x.Borrower)
                 .Include(x => x.TrxArchiveRent.Status)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.Creator.ArchiveUnit)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                 .Where(x => x.TrxArchiveRent.RetrievalDate != null)
                 .Where($"(Borrower.BorrowerName).Contains(@0)", model.searchValue)
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.Creator.ArchiveUnitId == User.ArchiveUnitId)))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                .Where(model.advanceSearch!.Search)
             .CountAsync();

            return result;
        }
        public async Task<IEnumerable<object>> GetRetrievalByArchiveRentId(Guid Id, string form)
        {
            var result = await _context.TrxArchiveRents
                .Include(x => x.TrxArchiveRentDetails.FirstOrDefault().Archive.SubSubjectClassification).ThenInclude(x => x.SubjectClassification.Classification)
                .Include(x => x.TrxArchiveRentDetails.FirstOrDefault().Archive.Creator.ArchiveUnit)
                .Include(x => x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive).ThenInclude(x => x.Row.Level.Rack.Room.Floor)
                .Include(x => x.TrxRentHistories).ThenInclude(x => x.Borrower)
                .Where(x => x.TrxArchiveRentId == Id && (form == "Add" ? x.StatusId == (int)GlobalConst.STATUS.WaitingForRetrieval : x.StatusId == (int)GlobalConst.STATUS.Retrieved))
                .Select(x => new {
                    UserNik = x.TrxRentHistories.FirstOrDefault().Borrower.BorrowerIdentityNumber,
                    UserName = x.TrxRentHistories.FirstOrDefault().Borrower.BorrowerName,
                    UserEmail = x.TrxRentHistories.FirstOrDefault().Borrower.BorrowerEmail,
                    UserPhone = x.TrxRentHistories.FirstOrDefault().Borrower.BorrowerPhone,
                    UserCompany = x.TrxRentHistories.FirstOrDefault().Borrower.BorrowerCompany,
                    ClassificationName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.SubSubjectClassification.SubjectClassification.Classification.ClassificationName,
                    ArchiveId = x.TrxArchiveRentDetails.FirstOrDefault().Archive.ArchiveId,
                    TitleArchive = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TitleArchive,
                    CreatorName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.Creator.CreatorName,
                    ArchiveUnit = x.TrxArchiveRentDetails.FirstOrDefault().Archive.Creator.ArchiveUnit.ArchiveUnitName,
                    RowName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.RowName,
                    LevelName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.LevelName,
                    RackName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RackName,
                    RoomName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.Room.RoomName,
                    FloorName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.Room.Floor.FloorName,
                    RequestedDate = x.RequestedDate,
                    RequestedReturnDate = x.RequestedReturnDate,
                    ArchiveSort = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().Sort
                })
                .ToListAsync();
                
            return result;
        

        }
        public async Task<IEnumerable<object>> GetRetrievalDetailByArchiveRentId(Guid ArchiveId, int sort)
        {
            var result = await _context.TrxMediaStorageInActiveDetails
                .Include(x => x.MediaStorageInActive)
                .Include(x => x.Archive.SubSubjectClassification).ThenInclude(x => x.SubjectClassification.Classification)
                .Include(x => x.Archive.Creator.ArchiveUnit)
                .Where(x => x.ArchiveId == ArchiveId || x.Sort == sort)
                .Select(x => new { 
                    ClassificationName = x.Archive.SubSubjectClassification.SubjectClassification.Classification.ClassificationName,
                    ArchiveId = x.Archive.ArchiveId,
                    TitleArchive = x.Archive.TitleArchive,
                    CreatorName = x.Archive.Creator.CreatorName,
                    ArchiveUnit = x.Archive.Creator.ArchiveUnit.ArchiveUnitName
                })
                .ToListAsync();

            return result;


        }
        public async Task<bool> ValidateQRBoxWithArchiveRentId(Guid ArchiveRentId, string mediaInActiveCode)
        {
            bool isValid = false;

            var result = await _context.TrxArchiveRents
                .Include(x => x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive)
                .Where(x => x.TrxArchiveRentId == ArchiveRentId && x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.MediaStorageInActiveCode == mediaInActiveCode)
                .ToListAsync();

            if (result.Count > 0)
                isValid = true;


            return isValid;


        }
        public async Task<bool> UpdateArchiveRent(Guid ArchiveRentId, Guid UserId) 
        {
            int status = 0;

            var result = await _context.TrxArchiveRents.FirstOrDefaultAsync(x => x.TrxArchiveRentId == ArchiveRentId);

            if (result != null)
            {
                if (result.StatusId == (int)GlobalConst.STATUS.WaitingForRetrieval)
                {
                    status = (int)GlobalConst.STATUS.Retrieved;
                    await _context.Database.ExecuteSqlAsync($"UPDATE TRX_ARCHIVE_RENT SET retrieval_date = {DateTime.Now}, status_id = {status}, updated_by ={UserId} WHERE trx_archive_rent_id = {ArchiveRentId}");
                }
                else
                {
                    status = (int)GlobalConst.STATUS.Return;
                    await _context.Database.ExecuteSqlAsync($"UPDATE TRX_ARCHIVE_RENT SET return_date = {DateTime.Now}, status_id = {status}, updated_by ={UserId} WHERE trx_archive_rent_id = {ArchiveRentId}");

                    var dataStorage = await _context.TrxMediaStorageInActiveDetails.FirstOrDefaultAsync(x => x.ArchiveId == result.ArchiveId);
                    if (dataStorage != null)
                    {
                        var dataSubStorage = await _context.TrxMediaStorageInActiveDetails.Where(x => x.Sort == dataStorage.Sort && x.MediaStorageInActiveId == dataStorage.MediaStorageInActiveId).ToListAsync();
                        if (dataSubStorage.Any())
                        {
                            foreach (TrxMediaStorageInActiveDetail item in dataSubStorage)
                            {
                                item.IsRent = true;
                                _context.TrxMediaStorageInActiveDetails.Update(item);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
        #region Return
        public async Task<IEnumerable<object>> GetReturnByFilterModel(DataTableModel model)
        {
            var User = AppUsers.CurrentUser(model.SessionUser);
            var result = await _context.TrxRentHistories
                 .Include(x => x.Borrower)
                 .Include(x => x.TrxArchiveRent.Status)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.Creator.ArchiveUnit)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                 .Where(x => x.TrxArchiveRent.ReturnDate != null)
                 .Where($"(Borrower.BorrowerName).Contains(@0)", model.searchValue)
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.Creator.ArchiveUnitId == User.ArchiveUnitId)))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                .Where(model.advanceSearch!.Search)
                 .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                 .Skip(model.skip).Take(model.pageSize)
                 .Select(x => new
                 {
                     x.TrxArchiveRentId,
                     Name = x.Borrower.BorrowerName,
                     Company = x.Borrower.BorrowerCompany,
                     UserCreatedBy = GetUserNameCreatedById(x.CreatedBy),
                     x.TrxArchiveRent.ApprovalDate,
                     x.TrxArchiveRent.ApprovalReturnDate,
                     x.TrxArchiveRent.ReturnDate,
                     Status = x.TrxArchiveRent.Status.Name,
                     Color = x.TrxArchiveRent.Status.Color,
                 })
                 .ToListAsync();

            return result;
        }

        public async Task<int> GetReturnCountByFilterModel(DataTableModel model)
        {
            var User = AppUsers.CurrentUser(model.SessionUser);
            var result = await _context.TrxRentHistories
                 .Include(x => x.Borrower)
                 .Include(x => x.TrxArchiveRent.Status)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.Creator.ArchiveUnit)
                .Include(x => x.TrxArchiveRent.TrxArchiveRentDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                 .Where(x => x.TrxArchiveRent.ReturnDate != null)
                 .Where($"(Borrower.BorrowerName).Contains(@0)", model.searchValue)
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.Creator.ArchiveUnitId == User.ArchiveUnitId)))
                .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveRent.TrxArchiveRentDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                .Where(model.advanceSearch!.Search)
                .CountAsync();

            return result;
        }

        public async Task<IEnumerable<object>> GetReturnByArchiveRentId(Guid Id, string form)
        {
            var result = await _context.TrxArchiveRents
                .Include(x => x.TrxRentHistories).ThenInclude(x => x.Borrower)
                .Include(x => x.TrxArchiveRentDetails.FirstOrDefault().Archive.SubSubjectClassification).ThenInclude(x => x.SubjectClassification.Classification)
                .Include(x => x.TrxArchiveRentDetails.FirstOrDefault().Archive.Creator.ArchiveUnit)
                .Include(x => x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive).ThenInclude(x => x.Row.Level.Rack.Room.Floor)
                .Where(x => x.TrxArchiveRentId == Id && (form == "Add" ? x.StatusId == (int)GlobalConst.STATUS.Retrieved : x.StatusId == (int)GlobalConst.STATUS.Return))
                .Select(x => new {
                    UserNik = x.TrxRentHistories.FirstOrDefault().Borrower.BorrowerIdentityNumber,
                    UserName = x.TrxRentHistories.FirstOrDefault().Borrower.BorrowerName,
                    UserEmail = x.TrxRentHistories.FirstOrDefault().Borrower.BorrowerEmail,
                    UserPhone = x.TrxRentHistories.FirstOrDefault().Borrower.BorrowerPhone,
                    UserCompany = x.TrxRentHistories.FirstOrDefault().Borrower.BorrowerCompany,
                    ClassificationName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.SubSubjectClassification.SubjectClassification.Classification.ClassificationName,
                    ArchiveId = x.TrxArchiveRentDetails.FirstOrDefault().Archive.ArchiveId,
                    TitleArchive = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TitleArchive,
                    CreatorName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.Creator.CreatorName,
                    ArchiveUnit = x.TrxArchiveRentDetails.FirstOrDefault().Archive.Creator.ArchiveUnit.ArchiveUnitName,
                    RowName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.RowName,
                    LevelName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.LevelName,
                    RackName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RackName,
                    RoomName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.Room.RoomName,
                    FloorName = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.Room.Floor.FloorName,
                    RequestedDate = x.RequestedDate,
                    RequestedReturnDate = x.RequestedReturnDate,
                    ArchiveSort = x.TrxArchiveRentDetails.FirstOrDefault().Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().Sort
                })
                .ToListAsync();

            return result;


        }
        public async Task<IEnumerable<object>> GetReturnDetailByArchiveRentId(Guid ArchiveId, int sort)
        {
            var result = await _context.TrxMediaStorageInActiveDetails
                .Include(x => x.MediaStorageInActive)
                .Include(x => x.Archive.SubSubjectClassification).ThenInclude(x => x.SubjectClassification.Classification)
                .Include(x => x.Archive.Creator.ArchiveUnit)
                .Where(x => x.ArchiveId == ArchiveId || x.Sort == sort)
                .Select(x => new {
                    ClassificationName = x.Archive.SubSubjectClassification.SubjectClassification.Classification.ClassificationName,
                    ArchiveId = x.Archive.ArchiveId,
                    TitleArchive = x.Archive.TitleArchive,
                    CreatorName = x.Archive.Creator.CreatorName,
                    ArchiveUnit = x.Archive.Creator.ArchiveUnit.ArchiveUnitName
                })
                .ToListAsync();

            return result;


        }

        public static string GetUserNameCreatedById(Guid Id)
        {
            BksArditaDevContext ctx = new BksArditaDevContext();
            var result = ctx.MstUsers.AsNoTracking()
                .Include(x => x.Employee)
                .FirstOrDefault(x => x.UserId == Id);
            return result.Employee.Name;
        }

        public async Task<IEnumerable<MstBorrower>> GetBorrower() 
        {
            var results = await _context.MstBorrowers
                .AsNoTracking()
                .ToListAsync();
            return results;
        }

        public async Task<IEnumerable<VwArchiveRentBox>> GetArchiveRentBoxById(Guid Id)
        {
            return await _context.VwArchiveRentBoxes.Where(x => x.TrxArchiveRentId == Id).ToListAsync();
        }
        #endregion

    }
}
