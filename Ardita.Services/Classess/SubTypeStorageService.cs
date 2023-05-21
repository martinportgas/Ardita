using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class SubTypeStorageService : ISubTypeStorageService
{
    private readonly ISubTypeStorageRepository _subTypeStorageRepository;
        
    public SubTypeStorageService(ISubTypeStorageRepository subTypeStorageRepository) => _subTypeStorageRepository = subTypeStorageRepository;
    
    public async Task<IEnumerable<MstSubTypeStorage>> GetAllByTypeStorageId(Guid ID) => await _subTypeStorageRepository.GetAllByTypeStorageId(ID);
}
