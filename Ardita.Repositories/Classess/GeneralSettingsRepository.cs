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
        var data = await _context.MstGeneralSettings.Where(x => x.IsActive == true).AsNoTracking().OrderBy(x => x.GeneralSettingsId).LastOrDefaultAsync();

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

    public Task<int> Update(MstGeneralSetting model, List<IdxGeneralSettingsFormatFile> details)
    {
        throw new NotImplementedException();
    }
}
