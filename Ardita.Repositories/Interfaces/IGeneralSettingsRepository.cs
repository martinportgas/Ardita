using Ardita.Models.DbModels;

namespace Ardita.Repositories.Interfaces;

public interface IGeneralSettingsRepository
{
    Task<int> Insert(MstGeneralSetting model, List<IdxGeneralSettingsFormatFile> details);
    Task<int> Update(MstGeneralSetting model, List<IdxGeneralSettingsFormatFile> details);
    Task<int> Delete(MstGeneralSetting model);
    Task<IEnumerable<MstGeneralSetting>> GetById(Guid id);
    Task<IEnumerable<MstGeneralSetting>> GetAll();
}
