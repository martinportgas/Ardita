using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Archive;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess;

public class ArchiveRepository : IArchiveRepository
{
    private readonly BksArditaDevContext _context;
    private readonly IConfiguration _configuration;
    private readonly string _whereClause = @"TitleArchive+TypeSender+Keyword+ActiveRetention.ToString()+CreatedDateArchive.ToString()
                                            +InactiveRetention.ToString()+Volume.ToString()+Gmd.GmdName+SubSubjectClassification.SubSubjectClassificationName
                                            +Creator.CreatorName+ArchiveType.ArchiveTypeName";

    public ArchiveRepository(BksArditaDevContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<int> Delete(TrxArchive model)
    {
        int result = 0;

        if (model.ArchiveId != Guid.Empty)
        {
            var data = await _context.TrxArchives.AsNoTracking().FirstAsync(x => x.ArchiveId == model.ArchiveId);
            if (data != null)
            {
                data.IsActive = false;
                data.UpdatedDate = model.UpdatedDate;
                data.UpdatedBy = model.UpdatedBy;
                _context.TrxArchives.Update(data);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }

    public async Task<IEnumerable<TrxArchive>> GetAll(List<string> listArchiveUnitCode = null)
    {
        listArchiveUnitCode = listArchiveUnitCode == null ? new List<string> { } : listArchiveUnitCode;
        return await _context.TrxArchives
            .Include(x => x.Gmd)
            .Include(x => x.SubSubjectClassification)
            .Include(x => x.SecurityClassification)
            .Include(x => x.Creator)
            .Include(x => x.ArchiveOwner)
            .Include(x => x.ArchiveType)
            .AsNoTracking()
            .Where($"{(listArchiveUnitCode.Count > 0 ? "@0.Contains(Creator.ArchiveUnit.ArchiveUnitCode)" : "1=1")} ", listArchiveUnitCode)
            .Where(x => x.IsActive == true)
            .Where(x => x.Gmd.IsActive == true)
            .Where(x => x.SubSubjectClassification.IsActive == true)
            .Where(x => x.SecurityClassification.IsActive == true)
            .Where(x => x.Creator.IsActive == true)
            .Where(x => x.ArchiveOwner.IsActive == true)
            .Where(x => x.ArchiveType.IsActive == true)
            .OrderByDescending(x => x.CreatedDate).ToListAsync();
    }
    public async Task<IEnumerable<TrxArchive>> GetAllInActive(List<string> listArchiveUnitCode)
    {
        return await _context.TrxArchives
            .Include(x => x.Gmd)
            .Include(x => x.SubSubjectClassification)
            .Include(x => x.SecurityClassification)
            .Include(x => x.Creator)
            .Include(x => x.ArchiveOwner)
            .Include(x => x.ArchiveType)
            .Include(x => x.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive)
            .AsNoTracking()
            .Where($"{(listArchiveUnitCode.Count > 0 ? "@0.Contains(Creator.ArchiveUnit.ArchiveUnitCode)" : "1=1")} ", listArchiveUnitCode)
            .Where(x => x.IsActive == true)
            .OrderByDescending(x => x.CreatedDate).ToListAsync();
    }

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        var result = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SecurityClassification)
                .Include(x => x.SubSubjectClassification).ThenInclude(x => x.TrxPermissionClassifications)
                .Include(x => x.Creator).ThenInclude(x => x.ArchiveUnit)
                .Include(x => x.ArchiveOwner)
                .Include(x => x.ArchiveType)
                .Where(x => x.IsActive == true)
                .Where($"({_whereClause}).Contains(@0) ", model.searchValue)
                .Where($"{(model.PositionId != null ? $"SubSubjectClassification.TrxPermissionClassifications.Any(PositionId.Equals(@0))" : "1=1")} ", model.PositionId)
                .Where($"{(model.listArchiveUnitCode.Count > 0 ? "@0.Contains(Creator.ArchiveUnit.ArchiveUnitCode)" : "1=1")} ", model.listArchiveUnitCode )
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new
                {
                    x.ArchiveId,
                    x.TypeSender,
                    x.Keyword,
                    x.TitleArchive,
                    CreatedDateArchive = x.CreatedDateArchive.ToString(),
                    x.ActiveRetention,
                    x.InactiveRetention,
                    x.Volume,
                    x.Gmd.GmdName,
                    x.SubSubjectClassification.SubSubjectClassificationName,
                    x.SecurityClassification.SecurityClassificationName,
                    x.Creator.CreatorName,
                    TypeArchive = x.ArchiveType.ArchiveTypeName,
                    x.StatusId,
                    x.Status.Color,
                    x.ArchiveCode,
                    Status = x.Status.Name,
                    x.DocumentNo
                })
                .ToListAsync();

        return result;
    }

    public async Task<TrxArchive> GetById(Guid id)
    {
        var result = await _context.TrxArchives.AsNoTracking()
            .Include(x => x.ArchiveOwner)
            .Include(x => x.ArchiveType)
            .Include(x => x.Gmd)
            .Include(x => x.SubSubjectClassification)
                .ThenInclude(c => c.Creator)
                .ThenInclude(au => au!.ArchiveUnit)
                .ThenInclude(cmp => cmp.Company)
            .Include(x => x.SecurityClassification)
            .Include(x => x.Creator)
            .Include(x => x.TrxFileArchiveDetails)
            .Include(x => x.TrxMediaStorageDetails)
                .ThenInclude(ms => ms.MediaStorage)
                .ThenInclude(r => r.Row)
                .ThenInclude(l => l.Level)
                .ThenInclude(r => r!.Rack)
                .ThenInclude(room => room!.Room)
                .ThenInclude(f => f!.Floor)
                .ThenInclude(aut => aut!.ArchiveUnit)
            .Include(x => x.TrxMediaStorageDetails)
                .ThenInclude(ms => ms.MediaStorage)
                .ThenInclude(ts => ts.TypeStorage)
            .Include(x => x.TrxMediaStorageInActiveDetails)
                .ThenInclude(ms => ms.MediaStorageInActive)
                .ThenInclude(r => r.Row)
                .ThenInclude(l => l.Level)
                .ThenInclude(r => r!.Rack)
                .ThenInclude(room => room!.Room)
                .ThenInclude(f => f!.Floor)
                .ThenInclude(aut => aut!.ArchiveUnit)
            .Include(x => x.TrxMediaStorageInActiveDetails)
                .ThenInclude(ms => ms.MediaStorageInActive)
                .ThenInclude(ts => ts.TypeStorage)
            .Where(x => x.ArchiveId == id && x.IsActive == true)
            .FirstOrDefaultAsync();

        return result;
    }

    public async Task<int> GetCount() => await _context.TrxArchives.CountAsync(x => x.IsActive == true);

    public async Task<int> GetCountAll() => await _context.TrxArchives.CountAsync();

    public async Task<int> GetCountByFilterData(DataTableModel model)
    {
        var result = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SecurityClassification)
                .Include(x => x.SubSubjectClassification).ThenInclude(x => x.TrxPermissionClassifications)
                .Include(x => x.Creator).ThenInclude(x => x.ArchiveUnit)
                .Include(x => x.ArchiveOwner)
                .Include(x => x.ArchiveType)
                .Where($"({_whereClause}).Contains(@0) ", model.searchValue)
                .Where($"{(model.PositionId != null ? $"SubSubjectClassification.TrxPermissionClassifications.Any(PositionId.Equals(@0))" : "1=1")} ", model.PositionId)
                .Where($"{(model.listArchiveUnitCode.Count > 0 ? "@0.Contains(Creator.ArchiveUnit.ArchiveUnitCode)" : "1=1")} ", model.listArchiveUnitCode)
                .CountAsync();

        return result;
    }

    public async Task<string> GetPathArchive(Guid SubSubjectClassificationId, DateTime CreatedDateArchive)
    {
        PathArchiveComponentsModel path = new();
        string pathResult = string.Empty;

        path = (from company in _context.MstCompanies
                join unitArchive in _context.TrxArchiveUnits on company.CompanyId equals unitArchive.CompanyId
                join creator in _context.MstCreators on unitArchive.ArchiveUnitId equals creator.ArchiveUnitId
                join subSubject in _context.TrxSubSubjectClassifications on creator.CreatorId equals subSubject.CreatorId
                where subSubject.SubSubjectClassificationId == SubSubjectClassificationId
                select new PathArchiveComponentsModel
                {
                    CompanyCode = company.CompanyCode,
                    ArchiveUnitCode = unitArchive.ArchiveUnitCode,
                    CreatorCode = creator.CreatorCode,
                    SubSubjectClassificationCode = subSubject.SubSubjectClassificationCode!
                }).FirstOrDefault();

        await Task.Delay(0);

        pathResult = $"{path.CompanyCode}\\{CreatedDateArchive:yyyy}\\{CreatedDateArchive:MM}\\{path.ArchiveUnitCode}\\{path.CreatorCode}\\{path.SubSubjectClassificationCode}\\";

        return pathResult;
    }
    public async Task<int> Submit(TrxArchive model)
    {
        int result = 0;

        if (model != null)
        {
            //using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var e in _context.ChangeTracker.Entries())
                {
                    e.State = EntityState.Detached;
                }

                _context.Entry(model).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                //await transaction.CommitAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        return result;
    }
    public async Task<int> Insert(TrxArchive model, List<FileModel> files, string path = "")
    {
        int result = 0;

        if (model != null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                model.IsActive = true;
                model.ArchiveCode = $"{_context.MstSecurityClassifications.FirstOrDefault(x => x.SecurityClassificationId == model.SecurityClassificationId)!.SecurityClassificationCode}.{model.CreatedDateArchive.Year}.{_context.MstCreators.FirstOrDefault(x => x.CreatorId == model.CreatorId)!.CreatorCode}.{(_context.TrxArchives.Count() + 1).ToString("D4")}";

                foreach (var e in _context.ChangeTracker.Entries())
                {
                    e.State = EntityState.Detached;
                }

                _context.Entry(model).State = EntityState.Added;
                result = await _context.SaveChangesAsync();

                if (result != 0 && files.Any())
                {
                    path = $"{path}\\{model.ArchiveCode}\\";
                    foreach (FileModel file in files)
                    {
                        Guid newId = Guid.NewGuid();
                        string ext = Path.GetExtension(file.FileName!);

                        TrxFileArchiveDetail temp = new()
                        {
                            FileArchiveDetailId = newId,
                            ArchiveId = model.ArchiveId,
                            FileName = file.FileName!,
                            FileNameEncrypt = string.Concat(newId, "_", DateTime.Now.ToString("ddMMyyyyHHmmssffff"), ext),
                            FilePath = path,
                            FileType = file.FileType!,
                            CreatedBy = model.CreatedBy,
                            CreatedDate = model.CreatedDate,
                            IsActive = true
                        };

                        _context.TrxFileArchiveDetails.Add(temp);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        Byte[] bytes = Convert.FromBase64String(file.Base64!);
                        File.WriteAllBytes(temp.FilePath + temp.FileNameEncrypt, bytes);
                    }
                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        return result;
    }

    public async Task<bool> InsertBulk(List<TrxArchive> trxArchives)
    {
        bool result = false;
        var lastCOunt = _context.TrxArchives.Count();
        if (trxArchives.Count() > 0)
        {
            foreach (var item in trxArchives)
            {
                lastCOunt = lastCOunt + 1;
                item.ArchiveCode = $"{_context.MstSecurityClassifications.FirstOrDefault(x => x.SecurityClassificationId == item.SecurityClassificationId)!.SecurityClassificationCode}.{item.CreatedDateArchive.Year}.{_context.MstCreators.FirstOrDefault(x => x.CreatorId == item.CreatorId)!.CreatorCode}{lastCOunt.ToString("D4")}";
            }

            await _context.AddRangeAsync(trxArchives);
            await _context.SaveChangesAsync();
            result = true;
        }
        return result;
    }

    public async Task<int> Update(TrxArchive model, List<FileModel> files, List<Guid> filesDeletedId, string path = "")
    {
        int result = 0;

        if (model != null && model.ArchiveId != Guid.Empty)
        {
            var data = await _context.TrxArchives.AsNoTracking().FirstAsync(x => x.ArchiveId == model.ArchiveId);
            if (data != null)
            {
                //Update Header
                model.IsActive = data.IsActive;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;

                var TrxSubSubjectClassifications = _context.TrxSubSubjectClassifications.FirstOrDefault(r => r.SubSubjectClassificationId == model.SubSubjectClassificationId);
                var MstSecurityClassifications = _context.MstSecurityClassifications.FirstOrDefault(r => r.SecurityClassificationId == model.SecurityClassificationId);
                var MstCreators = _context.MstCreators.FirstOrDefault(r => r.CreatorId == model.CreatorId);

                foreach (var e in _context.ChangeTracker.Entries())
                {
                    e.State = EntityState.Detached;
                }

                model.SubSubjectClassificationId = TrxSubSubjectClassifications!.SubSubjectClassificationId;
                model.SecurityClassificationId = MstSecurityClassifications!.SecurityClassificationId;
                model.CreatorId = MstCreators!.CreatorId;

                _context.Entry(model).State = EntityState.Modified;
                result = await _context.SaveChangesAsync();

                //Update Detail
                path = $"{path}\\{model.ArchiveCode}\\";
                if (result != 0 && files.Any())
                {
                    foreach (FileModel file in files)
                    {
                        Guid newId = Guid.NewGuid();
                        string ext = Path.GetExtension(file.FileName!);
                        TrxFileArchiveDetail temp = new()
                        {
                            ArchiveId = model.ArchiveId,
                            FileName = file.FileName!,
                            FileNameEncrypt = string.Concat(newId, "_", DateTime.Now.ToString("ddMMyyyyHHmmssffff"), ext),
                            FilePath = path,
                            FileType = file.FileType!,
                            CreatedBy = model.CreatedBy,
                            CreatedDate = model.CreatedDate,
                            IsActive = true
                        };

                        _context.TrxFileArchiveDetails.Add(temp);

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        Byte[] bytes = Convert.FromBase64String(file.Base64!);
                        File.WriteAllBytes(temp.FilePath + temp.FileNameEncrypt, bytes);
                    }
                    await _context.SaveChangesAsync();
                }

                //Delete Files
                List<TrxFileArchiveDetail> listFileDeleted = new();
                var listFileArchiveDetail = await _context.TrxFileArchiveDetails.Where(x => x.ArchiveId == model.ArchiveId && x.IsActive == true).ToListAsync();
                if (listFileArchiveDetail.Any())
                {
                    foreach(var item in listFileArchiveDetail)
                    {
                        if (filesDeletedId.Contains(item.FileArchiveDetailId))
                        {
                            if(File.Exists(item.FilePath))
                                File.Delete(item.FilePath);
                            listFileDeleted.Add(item);
                        }
                        else
                        {
                            if(item.FilePath != path)
                            {
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                string oldDirectory = string.Concat(item.FilePath, item.FileNameEncrypt);
                                string newDirectory = string.Concat(path, item.FileNameEncrypt);
                                if (Directory.Exists(item.FilePath))
                                {
                                    Directory.Move(oldDirectory, newDirectory);

                                    item.FilePath = path;
                                    _context.Update(item);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }

                _context.TrxFileArchiveDetails.RemoveRange(listFileDeleted);
                await _context.SaveChangesAsync();
            }
        }

        await Task.Delay(0);

        return result;
    }

    public async Task<IEnumerable<TrxArchive>> GetAvailableArchiveBySubSubjectId(Guid subjectId, Guid mediaStorageId = new Guid(), string monthYear = "")
    {
        string year = DateTime.Now.Year.ToString();
        string month = DateTime.Now.Month.ToString();
        if (!string.IsNullOrEmpty(monthYear))
        {
            string[] arrMonthYear = monthYear.Split('-');
            if(arrMonthYear.Length > 1)
            {
                year = arrMonthYear[0];
                month = arrMonthYear[1];
            }

        }
        var listNotAvailableArchive = from archive in _context.TrxArchives
                                      join mediaDetail in _context.TrxMediaStorageDetails on archive.ArchiveId equals mediaDetail.ArchiveId
                                      join media in _context.TrxMediaStorages on mediaDetail.MediaStorageId equals media.MediaStorageId
                                      where archive.IsActive == true && media.IsActive == true && mediaDetail.MediaStorageId != mediaStorageId
                                      select archive;

        int submit = (int)GlobalConst.STATUS.Submit;
        return await _context.TrxArchives
            .Include(x => x.SubSubjectClassification)
            .Include(x => x.ArchiveType)
            .Include(x => x.Creator)
            .Include(x => x.TrxMediaStorageDetails)
            .Where(x => x.IsActive == true && x.StatusId == submit && !listNotAvailableArchive.Contains(x) && x.SubSubjectClassification.SubjectClassificationId == subjectId)
            .Where($"{(string.IsNullOrEmpty(year) ? "1=1" : "CreatedDateArchive.Year == @0 && CreatedDateArchive.Month == @1")}", year, month)
            .OrderByDescending(x => x.TrxMediaStorageDetails.FirstOrDefault().MediaStorageId)
            .ToListAsync();
    }

    public async Task<IEnumerable<TrxArchive>> GetAvailableArchiveInActiveBySubSubjectId(Guid subSubjectId, Guid mediaStorageId = new Guid(), string year = "")
    {

        int submit = (int)GlobalConst.STATUS.Submit;

        var data = await _context.TrxArchives
            .Include(x => x.ArchiveType)
            .Include(x => x.Creator)
            .Include(x => x.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive)
            .Where(x => x.SubSubjectClassificationId == subSubjectId && x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.StatusId == submit)
            //.Where($"{"TrxMediaStorageInActiveDetails.MediaStorageInActive.StatusId == @0"}", submit)
            .ToListAsync();
        return data;
    }

    public Task<int> Submit(Guid ArchiveId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TrxArchive>> GetArchiveActiveBySubjectId(Guid subSubjectId)
    {
        var result = from archive in _context.TrxArchives
                     join mvmntDtl in _context.TrxArchiveMovementDetails on archive.ArchiveId equals mvmntDtl.ArchiveId
                     join mvmnt in _context.TrxArchiveMovements on mvmntDtl.ArchiveMovementId equals mvmnt.ArchiveMovementId
                     join media in _context.TrxMediaStorageInActiveDetails on archive.ArchiveId equals media.ArchiveId into tmp
                     from submedia in tmp.DefaultIfEmpty()
                     join mediaHeader in _context.TrxMediaStorageInActives on submedia.MediaStorageInActiveId equals mediaHeader.MediaStorageInActiveId into tmp2
                     from submediaHeader in tmp2.DefaultIfEmpty()
                     where submedia == null && (submediaHeader.IsActive != true)
                     where archive.SubSubjectClassificationId == subSubjectId
                     select archive;

        await Task.Delay(0);

        return result;
    }
}
