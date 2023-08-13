using Ardita.Models.DbModels;

namespace Ardita.Repositories.Interfaces;

public interface IGeneralSettingsRepository
{
    Task<int> Insert(MstGeneralSetting model, List<IdxGeneralSettingsFormatFile> details);
    Task<int> Update(MstGeneralSetting model, List<IdxGeneralSettingsFormatFile> details);
    Task<bool> IsExist();
    Task<MstGeneralSetting> GetExistingSettings();
}
