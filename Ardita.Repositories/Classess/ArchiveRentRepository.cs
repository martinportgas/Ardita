using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess
{
    public class ArchiveRentRepository : IArchiveRentRepository
    {
        private readonly BksArditaDevContext _context;
       
        public ArchiveRentRepository(BksArditaDevContext context)
        {
            _context = context;
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
            }
            return result;
        }

        public Task<int> Delete(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrxArchiveRent>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<object>> GetByBorrowerId(Guid Id)
        {
            var result = await _context.TrxRentHistories
                .Include(x => x.Borrower)
                .Include(x => x.TrxArchiveRent.Archive.Creator)
                .Include(x => x.TrxArchiveRent.Archive.Status)
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
                    ArchiveCode = x.TrxArchiveRent.Archive.ArchiveCode,
                    ArchiveTitle = x.TrxArchiveRent.Archive.TitleArchive,
                    CreatorName = x.TrxArchiveRent.Archive.Creator.CreatorName,
                    RentDate = x.TrxArchiveRent.RequestedDate,
                    ReturnDate = x.TrxArchiveRent.ReturnDate,
                    Status = x.TrxArchiveRent.Archive.Status.Name
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<object>> GetApprovalByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveRents
                 .Include(x => x.TrxRentHistories).ThenInclude(x => x.FirstOrDefault().Borrower)
                 .Include(x => x.Archive.Creator)
                 .Include(x => x.Status)
                 .Where(x => x.StatusId == 2)
                 .Where($"(User.Employee.Nik+User.Employee.Name).Contains(@0)", model.searchValue)
                 .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                 .Skip(model.skip).Take(model.pageSize)
                 .Select(x => new
                 {
                     x.TrxArchiveRentId,
                     
                     ArchiveTitle = x.Archive.TitleArchive,
                     CreatorName = x.Archive.Creator.CreatorName
                 })
                 .ToListAsync();

            return result;
        }

        public async Task<int> GetApprovalCountByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveRents
               .Include(x => x.TrxRentHistories.FirstOrDefault().Borrower)
               .Include(x => x.Archive.Creator)
               .Where(x => x.StatusId == 2)
               .Where($"(User.Employee.Nik+User.Employee.Name).Contains(@0)", model.searchValue)
               .CountAsync();

            return result;
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxRentHistories
                .Include(x => x.Borrower)
                .Include(x => x.TrxArchiveRent.Archive.Status)
                .Where($"(TrxArchiveRent.RequestedReturnDate.ToString()).Contains(@0)", model.searchValue)
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new
                {
                    x.TrxArchiveRentId,
                    Name = x.Borrower.BorrowerName,
                    x.TrxArchiveRent.RequestedDate,
                    x.TrxArchiveRent.RequestedReturnDate,
                    x.TrxArchiveRent.StatusId,
                    Status = x.TrxArchiveRent.Archive.Status.Name,
                    Color = x.TrxArchiveRent.Archive.Status.Color,
                    UserCreatedBy = GetUserNameCreatedById(x.CreatedBy)
                })
                .ToListAsync();

            return result;
        }

        public async Task<TrxArchiveRent> GetById(Guid id)
        {
            var result = await _context.TrxArchiveRents
                .Include(x => x.Archive.SubSubjectClassification)
                .Where(x => x.TrxArchiveRentId == id).ToListAsync();
            return result.FirstOrDefault();
        }
        public async Task<int> Insert(TrxArchiveRent model, MstBorrower borrower)
        {
            int result = 0;

            if (model != null)
            {
                model.TrxArchiveRentId = new Guid();
                model.StatusId = (int)GlobalConst.STATUS.ApprovalProcess;

                _context.TrxArchiveRents.Add(model);
                await _context.SaveChangesAsync();

                _context.MstBorrowers.Add(borrower);
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
            return result;
        }

        public async Task<int> Update(TrxArchiveRent model, MstBorrower borrower)
        {
            int result = 0;

            if (model != null)
            {
                model.TrxArchiveRentId = new Guid();
                model.StatusId = (int)GlobalConst.STATUS.ApprovalProcess;

            

                var borrowerResult = await _context.MstBorrowers.AsNoTracking().FirstOrDefaultAsync(x => x.BorrowerId == borrower.BorrowerId);

                if (borrowerResult != null) {
                    _context.TrxArchiveRents.Add(model);
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
            
            }
            return result;
        }
        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxRentHistories
                .Include(x => x.Borrower)
                .Include(x => x.TrxArchiveRent.Archive.Status)
                .Where($"(TrxArchiveRent.RequestedReturnDate.ToString()).Contains(@0)", model.searchValue)
               .Where($"(TrxArchiveRent.ReturnDate.ToString()).Contains(@0)", model.searchValue)
               .CountAsync();

            return result;
        }

        #region Retrieval
        public async Task<IEnumerable<object>> GetRetrievalByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveRents
               .Include(x => x.TrxRentHistories.FirstOrDefault().Borrower)
               .Include(x => x.Archive)
               .Include(x => x.Status)
               .Where(x => x.StatusId == (int)GlobalConst.STATUS.Retrieved)
               .Where($"(User.Employee.Name+RequestedDate.ToString()+RequestedReturnDate.ToString()).Contains(@0)", model.searchValue)
               .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
               .Skip(model.skip).Take(model.pageSize)
               .Select(x => new
               {
                   x.TrxArchiveRentId,
                  
                   x.RequestedDate,
                   x.RequestedReturnDate,
                   x.StatusId,
                   Status = x.Status.Name,
                   Color = x.Status.Color,
                   UserCreatedBy = GetUserNameCreatedById(x.CreatedBy)
               })
               .ToListAsync();

            return result;
        }

        public async Task<int> GetRetrievalCountByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveRents
             .Include(x => x.TrxRentHistories.FirstOrDefault().Borrower)
             .Include(x => x.Archive)
             .Include(x => x.Status)
             .Where(x => x.StatusId == (int)GlobalConst.STATUS.Retrieved)
             .Where($"(User.Employee.Name+RequestedDate.ToString()+ReturnDate.ToString()).Contains(@0)", model.searchValue)
             .CountAsync();

            return result;
        }
        public async Task<IEnumerable<object>> GetRetrievalByArchiveRentId(Guid Id, string form)
        {
            var result = await _context.TrxArchiveRents
                .Include(x => x.Archive.SubSubjectClassification).ThenInclude(x => x.SubjectClassification.Classification)
                .Include(x => x.Archive.Creator.ArchiveUnit)
                .Include(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive).ThenInclude(x => x.Row.Level.Rack.Room.Floor)
                .Where(x => x.TrxArchiveRentId == Id && (form == "Add" ? x.StatusId == (int)GlobalConst.STATUS.WaitingForRetrieval : x.StatusId == (int)GlobalConst.STATUS.Retrieved))
                .Select(x => new {
                    //UserNik = x.User.Employee.Nik,
                    //UserName = x.User.Employee.Name,
                    //UserEmail = x.User.Employee.Email,
                    //UserPhone = x.User.Employee.Phone,
                    //UserCompany = x.User.Employee.Company.CompanyName,
                    //UserRoleName = x.User.IdxUserRoles.FirstOrDefault().Role.Name,
                    ClassificationName = x.Archive.SubSubjectClassification.SubjectClassification.Classification.ClassificationName,
                    ArchiveId = x.Archive.ArchiveId,
                    TitleArchive = x.Archive.TitleArchive,
                    CreatorName = x.Archive.Creator.CreatorName,
                    ArchiveUnit = x.Archive.Creator.ArchiveUnit.ArchiveUnitName,
                    RowName = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.RowName,
                    LevelName = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.LevelName,
                    RackName = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RackName,
                    RoomName = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.Room.RoomName,
                    FloorName = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.Room.Floor.FloorName,
                    RequestedDate = x.RequestedDate,
                    RequestedReturnDate = x.RequestedReturnDate,
                    ArchiveSort = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().Sort
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
                .Include(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive)
                .Where(x => x.TrxArchiveRentId == ArchiveRentId && x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.MediaStorageInActiveCode == mediaInActiveCode)
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
                }
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
        #region Return
        public async Task<IEnumerable<object>> GetReturnByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveRents
               .Include(x => x.TrxRentHistories.FirstOrDefault().Borrower)
               .Include(x => x.Archive)
               .Include(x => x.Status)
               .Where(x => x.StatusId == (int)GlobalConst.STATUS.Return)
               .Where($"(User.Employee.Name+RequestedDate.ToString()+RequestedReturnDate.ToString()).Contains(@0)", model.searchValue)
               .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
               .Skip(model.skip).Take(model.pageSize)
               .Select(x => new
               {
                   x.TrxArchiveRentId,
                  // x.User.Employee.Name,
                   x.RequestedDate,
                   x.RequestedReturnDate,
                   x.StatusId,
                   Status = x.Status.Name,
                   Color = x.Status.Color,
                   UserCreatedBy = GetUserNameCreatedById(x.CreatedBy)
               })
               .ToListAsync();

            return result;
        }

        public async Task<int> GetReturnCountByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveRents
            .Include(x => x.TrxRentHistories.FirstOrDefault().Borrower)
            .Include(x => x.Archive)
            .Include(x => x.Status)
           .Where(x => x.StatusId == (int)GlobalConst.STATUS.Return)
            .Where($"(User.Employee.Name+RequestedDate.ToString()+ReturnDate.ToString()).Contains(@0)", model.searchValue)
            .CountAsync();

            return result;
        }

        public async Task<IEnumerable<object>> GetReturnByArchiveRentId(Guid Id, string form)
        {
            var result = await _context.TrxArchiveRents
                .Include(x => x.Archive.SubSubjectClassification).ThenInclude(x => x.SubjectClassification.Classification)
                .Include(x => x.Archive.Creator.ArchiveUnit)
                .Include(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive).ThenInclude(x => x.Row.Level.Rack.Room.Floor)
                .Where(x => x.TrxArchiveRentId == Id && (form == "Add" ? x.StatusId == (int)GlobalConst.STATUS.Retrieved : x.StatusId == (int)GlobalConst.STATUS.Return))
                .Select(x => new {
                    //UserNik = x.User.Employee.Nik,
                    //UserName = x.User.Employee.Name,
                    //UserEmail = x.User.Employee.Email,
                    //UserPhone = x.User.Employee.Phone,
                    //UserCompany = x.User.Employee.Company.CompanyName,
                    //UserRoleName = x.User.IdxUserRoles.FirstOrDefault().Role.Name,
                    ClassificationName = x.Archive.SubSubjectClassification.SubjectClassification.Classification.ClassificationName,
                    ArchiveId = x.Archive.ArchiveId,
                    TitleArchive = x.Archive.TitleArchive,
                    CreatorName = x.Archive.Creator.CreatorName,
                    ArchiveUnit = x.Archive.Creator.ArchiveUnit.ArchiveUnitName,
                    RowName = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.RowName,
                    LevelName = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.LevelName,
                    RackName = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RackName,
                    RoomName = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.Room.RoomName,
                    FloorName = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.Room.Floor.FloorName,
                    RequestedDate = x.RequestedDate,
                    RequestedReturnDate = x.RequestedReturnDate,
                    ArchiveSort = x.Archive.TrxMediaStorageInActiveDetails.FirstOrDefault().Sort
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
        #endregion

    }
}
