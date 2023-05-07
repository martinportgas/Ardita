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
                                            +Creator.CreatorName+TypeArchive";

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

    public async Task<IEnumerable<TrxArchive>> GetAll()
    {
        return await _context.TrxArchives
            .Include(x => x.Gmd)
            .Include(x => x.SubSubjectClassification)
            .Include(x => x.SecurityClassification)
            .Include(x => x.Creator)
            .AsNoTracking()
            .Where(x => x.IsActive == true).ToListAsync();
    }

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        var resultx = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SecurityClassification)
                .Include(x => x.SubSubjectClassification).ThenInclude(x => x.TrxPermissionClassifications)
                .Include(x => x.Creator)
                .Where($"({_whereClause}).Contains(@0) {(model.PositionId != null ? $"and SubSubjectClassification.TrxPermissionClassifications.Any(PositionId.Equals(@1))" : "")}", model.searchValue, model.PositionId)
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
                    x.Creator.CreatorName,
                    x.TypeArchive,
                    x.StatusId,
                    x.Status.Color,
                    x.ArchiveCode,
                    Status = x.Status.Name
                })
                .ToListAsync();

        return resultx;
    }

    public async Task<TrxArchive> GetById(Guid id)
    {
        var result = await _context.TrxArchives.AsNoTracking()
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
            .Where(x => x.ArchiveId == id && x.IsActive == true)
            .FirstAsync();

        return result;
    }

    public async Task<int> GetCount() => await _context.TrxArchives.CountAsync(x => x.IsActive == true);

    public async Task<int> GetCountAll() => await _context.TrxArchives.CountAsync();

    public async Task<int> GetCountByFilterData(DataTableModel model)
    {
        var resultx = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SecurityClassification)
                .Include(x => x.SubSubjectClassification).ThenInclude(x => x.TrxPermissionClassifications)
                .Include(x => x.Creator)
                .Where($"({_whereClause}).Contains(@0) {(model.PositionId != null ? $"and SubSubjectClassification.TrxPermissionClassifications.Any(PositionId.Equals(@1))" : "")}", model.searchValue, model.PositionId)
                .CountAsync();

        return resultx;
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

    public async Task<int> Insert(TrxArchive model, List<FileModel> files)
    {
        int result = 0;

        if (model != null)
        {
            string path = $"{model.CreatedDate:yyyy}\\{model.CreatedDate:MM}\\{model.CreatedDate:dd}\\";
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                model.IsActive = true;
                model.ArchiveCode = $"ARCHIVE-{_context.TrxArchives.Count() + 1}";

                foreach (var e in _context.ChangeTracker.Entries())
                {
                    e.State = EntityState.Detached;
                }

                _context.Entry(model).State = EntityState.Added;
                result = await _context.SaveChangesAsync();

                if (result != 0 && files.Any())
                {
                    foreach (FileModel file in files)
                    {
                        var directory = _configuration[GlobalConst.BASE_PATH_TEMP_ARCHIVE];

                        TrxFileArchiveDetail temp = new()
                        {
                            ArchiveId = model.ArchiveId,
                            FileName = file.FileName!,
                            FilePath = directory + path + model.ArchiveCode + "\\" + file.FileName,
                            FileType = file.FileType!,
                            CreatedBy = model.CreatedBy,
                            CreatedDate = model.CreatedDate,
                            IsActive = true
                        };

                        _context.TrxFileArchiveDetails.Add(temp);
                        directory = directory + path + model.ArchiveCode;

                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        Byte[] bytes = Convert.FromBase64String(file.Base64!);
                        File.WriteAllBytes(temp.FilePath, bytes);
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
                item.ArchiveCode = $"ARCHIVE-{lastCOunt}";
            }

            await _context.AddRangeAsync(trxArchives);
            await _context.SaveChangesAsync();
            result = true;
        }
        return result;
    }

    public async Task<int> Update(TrxArchive model, List<FileModel> files, List<Guid> filesDeletedId)
    {
        int result = 0;

        if (model != null && model.ArchiveId != Guid.Empty)
        {
            var data = await _context.TrxArchives.AsNoTracking().FirstAsync(x => x.ArchiveId == model.ArchiveId);
            if (data != null)
            {
                string path = $"{data.CreatedDate:yyyy}\\{data.CreatedDate:MM}\\{data.CreatedDate:dd}\\";

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
                if (result != 0 && files.Any())
                {
                    var directory = _configuration[GlobalConst.BASE_PATH_TEMP_ARCHIVE];

                    foreach (FileModel file in files)
                    {
                        TrxFileArchiveDetail temp = new()
                        {
                            ArchiveId = model.ArchiveId,
                            FileName = file.FileName!,
                            FilePath = directory + path + model.ArchiveCode + "\\" + file.FileName,
                            FileType = file.FileType!,
                            CreatedBy = model.CreatedBy,
                            CreatedDate = model.CreatedDate,
                            IsActive = true
                        };

                        _context.TrxFileArchiveDetails.Add(temp);
                        directory = directory + path + model.ArchiveCode;

                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        Byte[] bytes = Convert.FromBase64String(file.Base64!);
                        File.WriteAllBytes(temp.FilePath, bytes);
                    }
                    await _context.SaveChangesAsync();
                }

                //Delete Files
                List<TrxFileArchiveDetail> listFileDeleted = new();
                foreach (var item in filesDeletedId)
                {
                    TrxFileArchiveDetail temp = new();
                    temp = await _context.TrxFileArchiveDetails.AsNoTracking().FirstAsync(x => x.FileArchiveDetailId == item);
                    File.Delete(temp.FilePath);
                    listFileDeleted.Add(temp);
                }

                _context.TrxFileArchiveDetails.RemoveRange(listFileDeleted);
                await _context.SaveChangesAsync();
            }
        }

        await Task.Delay(0);

        return result;
    }

    public async Task<IEnumerable<TrxArchive>> GetAvailableArchiveBySubSubjectId(Guid subSubjectId)
    {
        var listNotAvailableArchive = from archive in _context.TrxArchives
                                      join mediaDetail in _context.TrxMediaStorageDetails on archive.ArchiveId equals mediaDetail.ArchiveId
                                      join media in _context.TrxMediaStorages on mediaDetail.MediaStorageId equals media.MediaStorageId
                                      where archive.IsActive == true && media.IsActive == true
                                      select archive;

        return await _context.TrxArchives.Where(x => x.IsActive == true && !listNotAvailableArchive.Contains(x) && x.SubSubjectClassificationId == subSubjectId).ToListAsync();

    }

    public Task<int> Submit(Guid ArchiveId)
    {
        throw new NotImplementedException();
    }
}
