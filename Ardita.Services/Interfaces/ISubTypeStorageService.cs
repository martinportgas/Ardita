using Ardita.Models.DbModels;

namespace Ardita.Services.Interfaces;

public interface ISubTypeStorageService
{
    Task<IEnumerable<MstSubTypeStorage>> GetAllByTypeStorageId(Guid ID);
}
