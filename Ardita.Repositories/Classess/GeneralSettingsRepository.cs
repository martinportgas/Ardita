using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ardita.Repositories.Classess;

public class GeneralSettingsRepository : IGeneralSettingsRepository
{
    private readonly BksArditaDevContext _context;
    public GeneralSettingsRepository(BksArditaDevContext context) => _context = context;

    public async Task<MstGeneralSetting> GetExistingSettings()
    {
        var data = await _context.MstGeneralSettings
            .Include(x => x.IdxGeneralSettingsFormatFiles)
            .Where(x => x.IsActive == true)
            .AsNoTracking()
            .OrderBy(x => x.GeneralSettingsId)
            .LastOrDefaultAsync();

        return data ?? new MstGeneralSetting();
    }

    public async Task<int> Insert(MstGeneralSetting model, List<IdxGeneralSettingsFormatFile> details)
    {
        int result = 0;
        List<IdxGeneralSettingsFormatFile> list = new();

        if (model is not null)
        {
            model.IsActive = true;
            _context.Add(model);
            result = await _context.SaveChangesAsync();

            foreach (var item in details)
            {
                item.GeneralSettingsId = model.GeneralSettingsId;
                list.Add(item);
            }

            _context.AddRange(list);
            result += await _context.SaveChangesAsync();
        }

        return result;
    }

    public async Task<bool> IsExist()
    {
        bool result = false;

        var data = await _context.MstGeneralSettings.Where(x => x.IsActive == true).ToListAsync();

        if (data.Any())
        {
            result = true;
        }

        return result;
    }

    public async Task<int> Update(MstGeneralSetting model, List<IdxGeneralSettingsFormatFile> details)
    {
        int result = 0;
        List<IdxGeneralSettingsFormatFile> oldDetails = new();
        List<IdxGeneralSettingsFormatFile> newDetails = new();


        if (model != null && model.GeneralSettingsId != Guid.Empty) 
        {
            //update header process
            var data = await _context.MstGeneralSettings.AsNoTracking().FirstOrDefaultAsync(x => x.GeneralSettingsId == model.GeneralSettingsId);

            if (model.SiteLogoContent is null) 
            {
                model.SiteLogoContent = data!.SiteLogoContent;
                model.SiteLogoFileName = data.SiteLogoFileName;
                model.SiteLogoFileType = data.SiteLogoFileType;
            }

            if (model.FavIconContent is null)
            {
                model.FavIconContent = data!.FavIconContent;
                model.FavIconFileName = data.FavIconFileName;
                model.FavIconFileType = data.FavIconFileType;

            }

            if (model.CompanyLogoContent is null)
            {
                model.CompanyLogoContent = data!.CompanyLogoContent;
                model.CompanyLogoFileName = data.CompanyLogoFileName;
                model.CompanyLogoFileType = data.CompanyLogoFileType;
            }

            model.CreatedBy = data!.CreatedBy;
            model.CreatedDate = data.CreatedDate;
            model.IsActive = data.IsActive;

            _context.Update(model);
            result = await _context.SaveChangesAsync();

            //update detail process
            oldDetails = await _context.IdxGeneralSettingsFormatFiles.AsNoTracking().Where(x => x.GeneralSettingsId == model.GeneralSettingsId).ToListAsync();

            if (oldDetails.Any())
            {
                _context.RemoveRange(oldDetails);
                result += await _context.SaveChangesAsync();
            }

            foreach (var item in details)
            {
                item.GeneralSettingsId = model.GeneralSettingsId;
                newDetails.Add(item);
            }

            _context.AddRange(newDetails);
            result += await _context.SaveChangesAsync();
        }

        return result;
    }
}
