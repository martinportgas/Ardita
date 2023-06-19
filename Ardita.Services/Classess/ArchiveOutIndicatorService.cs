using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ardita.Services.Classess;

public class ArchiveOutIndicatorService : IArchiveOutIndicatorService
{
    private readonly IArchiveOutIndicatorRepository _ArchiveOutIndicatorRepository;
    public ArchiveOutIndicatorService(IArchiveOutIndicatorRepository ArchiveOutIndicatorRepository)
    {
        _ArchiveOutIndicatorRepository = ArchiveOutIndicatorRepository;
    }
    public async Task<TrxArchiveOutIndicator> GetByUseAndDate(Guid archiveId, string usedBy, DateTime usedDate)
    {
        return await _ArchiveOutIndicatorRepository.GetByUseAndDate(archiveId, usedBy, usedDate);
    }
    public async Task<IEnumerable<TrxArchiveOutIndicator>> GetByMediaStorageId(Guid mediaStorageId)
    {
        return await _ArchiveOutIndicatorRepository.GetByMediaStorageId(mediaStorageId);
    }

    public async Task<int> Insert(TrxArchiveOutIndicator model)
    {
        return await _ArchiveOutIndicatorRepository.Insert(model);
    }

    public async Task<bool> InsertBulk(List<TrxArchiveOutIndicator> models)
    {
        return await _ArchiveOutIndicatorRepository.InsertBulk(models);
    }

    public async Task<bool> Process(Guid mediaStorageId, string detailIsUsed, string usedBy, string usedDate, string returnDate, Guid userId)
    {
        bool result = true;

        Guid archiveUsedId = new Guid(detailIsUsed);
        DateTime useDate = DateTime.Parse(usedDate);

        var data = await _ArchiveOutIndicatorRepository.GetByMediaStorageId(mediaStorageId);
        if (string.IsNullOrEmpty(returnDate))
        {
            var activeData = data.Where(x => x.IsActive == true && x.ArchiveId == archiveUsedId).FirstOrDefault();
            if (activeData != null)
            {
                if(activeData.ReturnDate != null)
                {
                    activeData.IsActive = false;
                    activeData.UpdatedBy = userId;
                    activeData.UpdatedDate = DateTime.Now;
                    await Update(activeData);

                    TrxArchiveOutIndicator item = new TrxArchiveOutIndicator();
                    item.MediaStorageId = mediaStorageId;
                    item.ArchiveId = archiveUsedId;
                    item.UsedBy = usedBy;
                    item.UsedDate = useDate;
                    item.IsActive = true;
                    item.CreatedBy = userId;
                    item.CreatedDate = DateTime.Now;
                    await Insert(item);
                }
            }
        }
        else
        {
            result = false;
            var activeData = data.Where(x => x.IsActive == true && x.ArchiveId == archiveUsedId).FirstOrDefault();
            if (activeData != null)
            {
                activeData.ReturnDate = DateTime.Parse(returnDate);
                activeData.UpdatedBy = userId;
                activeData.UpdatedDate = DateTime.Now;
                await Update(activeData);
            }
            else
            {
                TrxArchiveOutIndicator item = new TrxArchiveOutIndicator();
                item.MediaStorageId = mediaStorageId;
                item.ArchiveId = archiveUsedId;
                item.UsedBy = usedBy;
                item.UsedDate = useDate;
                item.ReturnDate = DateTime.Parse(returnDate);
                item.IsActive = true;
                item.CreatedBy = userId;
                item.CreatedDate = DateTime.Now;
                item.UpdatedBy = userId;
                item.UpdatedDate = DateTime.Now;
                await Insert(item);
            }
        }
        return result;
    }

    public async Task<int> Update(TrxArchiveOutIndicator model)
    {
        return await _ArchiveOutIndicatorRepository.Update(model);
    }
}
