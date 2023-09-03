using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess
{
    public class ArchiveMovementRepository : IArchiveMovementRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;

        public ArchiveMovementRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }
        public async Task<int> Delete(TrxArchiveMovement model)
        {
            int result = 0;

            if (model.ArchiveMovementId != Guid.Empty)
            {
                var data = await _context.TrxArchiveMovements.AsNoTracking().FirstAsync(x => x.ArchiveMovementId == model.ArchiveMovementId);
                if (data != null)
                {
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<TrxArchiveMovement>(GlobalConst.Delete, (Guid)model!.UpdatedBy!, new List<TrxArchiveMovement> { data }, new List<TrxArchiveMovement> {  });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
        public async Task<int> Submit(TrxArchiveMovement model)
        {
            int result = 0;

            if (model.ArchiveMovementId != Guid.Empty)
            {
                var data = await _context.TrxArchiveMovements.AsNoTracking().FirstAsync(x => x.ArchiveMovementId == model.ArchiveMovementId);
                if (data != null)
                {
                    data.Note = model.Note;
                    data.ApproveLevel = model.ApproveLevel;
                    data.IsActive = model.IsActive;
                    data.StatusId = model.StatusId;
                    data.StatusReceived = model.StatusReceived;
                    data.UpdatedBy = model.UpdatedBy;
                    data.UpdatedDate = model.UpdatedDate;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<TrxArchiveMovement>(GlobalConst.Update, (Guid)model!.UpdatedBy!, new List<TrxArchiveMovement> { data }, new List<TrxArchiveMovement> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<TrxArchiveMovement>> GetAll(string par = " 1=1 ")
        {
            var results = await _context.TrxArchiveMovements.Where(x => x.IsActive == true)
                .Where(par).AsNoTracking().ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxArchiveMovements.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<TrxArchiveMovement> GetById(Guid id)
        {
            var result = await _context.TrxArchiveMovements.Include(x => x.GmdDetail).Include(x => x.ArchiveUnitIdFromNavigation.Company).Include(x => x.ArchiveUnitIdDestinationNavigation.Company).AsNoTracking().FirstOrDefaultAsync(x => x.ArchiveMovementId == id);
            return result;
        }
        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            //var sortMove = model.sortColumn;
            //var sortDestroy = model.sortColumn;
            //switch (model.sortColumn)
            //{
            //    case "code":
            //        sortMove = nameof(TrxArchiveMovement.MovementCode);
            //        sortDestroy = nameof(TrxArchiveDestroy.DestroyCode);
            //        break;
            //    case "id":
            //        sortMove = nameof(TrxArchiveMovement.ArchiveMovementId);
            //        sortDestroy = nameof(TrxArchiveDestroy.ArchiveDestroyId);
            //        break;
            //    case "name":
            //        sortMove = nameof(TrxArchiveMovement.MovementName);
            //        sortDestroy = nameof(TrxArchiveDestroy.DestroyName);
            //        break;
            //    default:
            //        sortMove = model.sortColumn;
            //        sortDestroy = model.sortColumn;
            //        break;
            //}
            var User = AppUsers.CurrentUser(model.SessionUser);
            long? statusReceived = (int)GlobalConst.STATUS.ArchiveNotReceived;
            var result = await 
                    _context.TrxArchiveMovements
                    .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.SubSubjectClassification.SubjectClassification.Classification)
                    .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room.Floor)
                    .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                    .Include(x => x.Status)
                    .Where(x => x.IsActive == true && (x.MovementCode + x.MovementName + x.Note + x.Status.Name).Contains(model.searchValue))
                    .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitIdFrom == User.ArchiveUnitId))
                    .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveMovementDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                    .Where(model.advanceSearch!.Search)
                    .Select(x => new {
                        Id = x.ArchiveMovementId,
                        x.DocumentCode,
                        Code = x.MovementCode,
                        Name = x.MovementName,
                        x.StatusId,
                        x.StatusReceived,
                        x.Note,
                        x.Status.Color,
                        Status = x.Status.Name,
                        x.CreatedDate,
                        type = GlobalConst.ArsipPemindahan
                    })
                .Union(
                    _context.TrxArchiveDestroys
                    .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.SubSubjectClassification.SubjectClassification.Classification)
                    .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room.Floor)
                    .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                    .Include(x => x.Status)
                    .Where(x => x.IsActive == true)
                    .Where(x => x.IsActive == true && (x.DestroyCode + x.DestroyName + x.Note + x.Status.Name).Contains(model.searchValue))
                    .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitId == User.ArchiveUnitId))
                    .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveDestroyDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                    .Where(model.advanceSearch!.SearchOther)
                    .Select(x => new {
                        Id = x.ArchiveDestroyId,
                        x.DocumentCode,
                        Code = x.DestroyCode,
                        Name = x.DestroyName,
                        x.StatusId,
                        StatusReceived = statusReceived,
                        x.Note,
                        x.Status.Color,
                        Status = x.Status.Name,
                        x.CreatedDate,
                        type = GlobalConst.ArsipPemusnahan
                    })
                )
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();

            //var resultDestroy = await _context.TrxArchiveDestroys
            //    .Include(x => x.Status)
            //    .Where(x => x.IsActive == true)
            //    .Where(x => x.IsActive == true && (x.DestroyCode + x.DestroyName + x.Note + x.Status.Name).Contains(model.searchValue))
            //    .OrderBy($"{sortDestroy} {model.sortColumnDirection}")
            //    .Skip(model.skip).Take(model.pageSize)
            //    .Select(x => new {
            //        Id = x.ArchiveDestroyId,
            //        x.DocumentCode,
            //        Code = x.DestroyCode,
            //        Name = x.DestroyName,
            //        x.StatusId,
            //        x.Note,
            //        Color = x.Status.Color,
            //        Status = x.Status.Name,
            //        x.CreatedDate,
            //        type = "Arsip Pemusnahan"
            //    })
            //    .ToListAsync();

            //var result = resultMovement.Union(resultDestroy);

            return result;
        }
        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var User = AppUsers.CurrentUser(model.SessionUser);
            var result = await
                    _context.TrxArchiveMovements
                    .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.SubSubjectClassification.SubjectClassification.Classification)
                    .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room.Floor)
                    .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                    .Include(x => x.Status)
                    .Where(x => x.IsActive == true && (x.MovementCode + x.MovementName + x.Note + x.Status.Name).Contains(model.searchValue))
                    .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitIdFrom == User.ArchiveUnitId))
                    .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveMovementDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                    .Where(model.advanceSearch!.Search)
                    .Select(x => new {
                        Id = x.ArchiveMovementId,
                        x.DocumentCode,
                        Code = x.MovementCode,
                        Name = x.MovementName,
                        x.StatusId,
                        x.Note,
                        x.Status.Color,
                        Status = x.Status.Name,
                        x.CreatedDate,
                        type = GlobalConst.ArsipPemindahan
                    })
                .Union(
                    _context.TrxArchiveDestroys
                    .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.SubSubjectClassification.SubjectClassification.Classification)
                    .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room.Floor)
                    .Include(x => x.TrxArchiveDestroyDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                    .Include(x => x.Status)
                    .Where(x => x.IsActive == true)
                    .Where(x => x.IsActive == true && (x.DestroyCode + x.DestroyName + x.Note + x.Status.Name).Contains(model.searchValue))
                    .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitId == User.ArchiveUnitId))
                    .Where(x => (User.CreatorId == Guid.Empty ? true : x.TrxArchiveDestroyDetails.Any(x => x.Archive.CreatorId == User.CreatorId)))
                    .Where(model.advanceSearch!.SearchOther)
                    .Select(x => new {
                        Id = x.ArchiveDestroyId,
                        x.DocumentCode,
                        Code = x.DestroyCode,
                        Name = x.DestroyName,
                        x.StatusId,
                        x.Note,
                        x.Status.Color,
                        Status = x.Status.Name,
                        x.CreatedDate,
                        type = GlobalConst.ArsipPemusnahan
                    })
                )
                .CountAsync();

            return result;
        }

        public async Task<int> Insert(TrxArchiveMovement model)
        {
            int result = 0;

            var count = await _context.TrxArchiveMovements.CountAsync() + 1;

            if (model != null)
            {
                var archiveUnit = await _context.TrxArchiveUnits.FirstOrDefaultAsync(x => x.ArchiveUnitId == model.ArchiveUnitIdFrom);
                var company = await _context.MstCompanies.FirstOrDefaultAsync(x => x.CompanyId == archiveUnit!.CompanyId);

                model.IsActive = true;
                model.MovementCode = $"MOVE.{count.ToString("D3")}/{DateTime.Now.Month.ToString("D2")}/{DateTime.Now.Year}";
                model.DocumentCode = $"PD.{count.ToString("D3")}-{company!.CompanyCode}/{archiveUnit!.ArchiveUnitCode}/{DateTime.Now.Month.ToString("D2")}/{model.CreatedDate.Year}";
                _context.TrxArchiveMovements.Add(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxArchiveMovement>(GlobalConst.New, (Guid)model!.CreatedBy!, new List<TrxArchiveMovement> {  }, new List<TrxArchiveMovement> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxArchiveMovement> models)
        {
            bool result = false;
            if (models.Count() > 0)
            {
                await _context.AddRangeAsync(models);
                await _context.SaveChangesAsync();
                result = true;

                //Log
                if (result)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxArchiveMovement>(GlobalConst.New, (Guid)models.FirstOrDefault()!.CreatedBy!, new List<TrxArchiveMovement> {  }, models);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
        public async Task<int> Update(TrxArchiveMovement model)
        {
            int result = 0;

            if (model != null && model.ArchiveMovementId != Guid.Empty)
            {
                var data = await _context.TrxArchiveMovements.AsNoTracking().FirstAsync(x => x.ArchiveMovementId == model.ArchiveMovementId);
                if (data != null)
                {
                    model.GmdDetail = null;
                    model.ArchiveUnitIdFromNavigation = null;
                    model.ArchiveUnitIdDestinationNavigation = null;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<TrxArchiveMovement>(GlobalConst.Update, (Guid)model!.UpdatedBy!, new List<TrxArchiveMovement> { data }, new List<TrxArchiveMovement> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
    }
}
