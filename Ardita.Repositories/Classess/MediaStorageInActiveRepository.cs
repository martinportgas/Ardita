using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess;

public class MediaStorageInActiveRepository : IMediaStorageInActiveRepository
{
    private readonly BksArditaDevContext _context;
    public MediaStorageInActiveRepository(BksArditaDevContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        return await _context.TrxMediaStorageInActives
            .Include(x => x.SubSubjectClassification)
            .Include(x => x.Status)
            .Where(x => x.IsActive == true && (x.MediaStorageInActiveCode + x.ArchiveYear + x.StatusId + x.SubSubjectClassification.SubSubjectClassificationName).Contains(model.searchValue))
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.MediaStorageInActiveId,
                    x.MediaStorageInActiveCode,
                    x.SubSubjectClassification.SubSubjectClassificationName,
                    x.StatusId,
                    x.Status.Color,
                    Status = x.Status.Name
                })
                .ToListAsync();
    }

    public async Task<int> GetCountByFilterModel(DataTableModel model)
    {
        return await _context.TrxMediaStorageInActives
            .Include(x => x.SubSubjectClassification)
            .Include(x => x.Status)
            .Where(x => x.IsActive == true && (x.MediaStorageInActiveCode + x.ArchiveYear + x.StatusId + x.SubSubjectClassification.SubSubjectClassificationName).Contains(model.searchValue))
            .CountAsync();
    }

    public async Task<IEnumerable<object>> GetDetailArchive(Guid id)
    {
        var data = await _context.TrxArchives
            .Include(x => x.Creator.ArchiveUnit)
            .AsNoTracking()
            .Where(x => x.ArchiveId == id).ToListAsync();
            
        return data;
    }

    public async Task<TrxMediaStorageInActiveDetail> GetDetailByArchiveId(Guid id)
    {
        var result = await _context.TrxMediaStorageInActiveDetails
            .Include(x => x.MediaStorageInActive.TypeStorage.ArchiveUnit)
            .FirstOrDefaultAsync(x=>x.ArchiveId == id);
        return result;
    }

    public async Task<IEnumerable<object>> GetDetailByArchiveIdAndSort(Guid id, int sort)
    {
        var results = from trxMediaStorageInActive in _context.TrxMediaStorageInActives
                      join trxMediaStorageInActiveDetails in _context.TrxMediaStorageInActiveDetails
                        on trxMediaStorageInActive.MediaStorageInActiveId equals trxMediaStorageInActiveDetails.MediaStorageInActiveId
                        into mediaStorageInActiveDetails
                      from detail in mediaStorageInActiveDetails.DefaultIfEmpty()

                      join trxArchive in _context.TrxArchives on detail.ArchiveId equals trxArchive.ArchiveId
                      into archives
                      from archive in archives.DefaultIfEmpty()

                      join trxTypeStorage in _context.TrxTypeStorages on trxMediaStorageInActive.TypeStorageId equals trxTypeStorage.TypeStorageId
                      into typeStorages
                      from typeStorage in typeStorages.DefaultIfEmpty()

                      join idxSubTypeStorage in _context.IdxSubTypeStorages on typeStorage.TypeStorageId equals idxSubTypeStorage.TypeStorageId
                      into subTypeStorages
                      from subTypeStorage in subTypeStorages.DefaultIfEmpty()

                      join mstSubTypeStorage in _context.MstSubTypeStorages on subTypeStorage.SubTypeStorageId equals mstSubTypeStorage.SubTypeStorageId
                      into mSubTypeStorages
                      from mSubTypeStorage in mSubTypeStorages.DefaultIfEmpty()

                      join trxArchiveUnit in _context.TrxArchiveUnits on typeStorage.ArchiveUnitId equals trxArchiveUnit.ArchiveUnitId
                      into archiveUnits
                      from archiveUnit in archiveUnits.DefaultIfEmpty()

                      join trxSubSubjectClassification in _context.TrxSubSubjectClassifications on trxMediaStorageInActive.SubSubjectClassificationId
                      equals trxSubSubjectClassification.SubSubjectClassificationId
                      into subSubjectClassifications
                      from subSubjectClassification in subSubjectClassifications.DefaultIfEmpty()

                      join trxSubjectClassification in _context.TrxSubjectClassifications on subSubjectClassification.SubjectClassificationId
                      equals trxSubjectClassification.SubjectClassificationId
                      into subjectClassifications
                      from subjectClassification in subjectClassifications.DefaultIfEmpty()

                      join mstCreator in _context.MstCreators on subSubjectClassification.CreatorId equals mstCreator.CreatorId
                      into creators
                      from creator in creators.DefaultIfEmpty()
                      where detail.Sort == sort && trxMediaStorageInActive.MediaStorageInActiveId == id
                      select new
                      {
                          MediaStorageInActiveDetailId = detail.MediaStorageInActiveDetailId,
                          MediaStorageInActiveId = trxMediaStorageInActive.MediaStorageInActiveId,
                          MediaStorageInActiveCode = trxMediaStorageInActive.MediaStorageInActiveCode,
                          StorageName = detail.SubTypeStorageId == null ? typeStorage.TypeStorageName : mSubTypeStorage.SubTypeStorageName,
                          Sort = detail.Sort,
                          SubTypeStorageId = mSubTypeStorage.SubTypeStorageId,
                          ClassificationName = subjectClassification.SubjectClassificationName,
                          TitleArchive = archive.TitleArchive,
                          ArchiveId = detail.ArchiveId,
                          ArchiveName = archive.TitleArchive,
                          ArchiveUnitName = archiveUnit.ArchiveUnitName,
                          CreatorId = creator.CreatorId,
                          CreatorName = creator.CreatorName
                      };

        return results;

    }
}
