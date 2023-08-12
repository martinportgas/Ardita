using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess;

public class GeneralSettingsRepository : IGeneralSettingsRepository
{
    private readonly BksArditaDevContext _context;
    public GeneralSettingsRepository(BksArditaDevContext context) => _context = context;

    public Task<int> Delete(MstGeneralSetting model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MstGeneralSetting>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<MstGeneralSetting>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<int> Insert(MstGeneralSetting model, List<IdxGeneralSettingsFormatFile> details)
    {
        throw new NotImplementedException();
    }

    public Task<int> Update(MstGeneralSetting model, List<IdxGeneralSettingsFormatFile> details)
    {
        throw new NotImplementedException();
    }
}
