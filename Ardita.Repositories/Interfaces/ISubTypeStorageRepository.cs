using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface ISubTypeStorageRepository
{
    Task<IEnumerable<MstSubTypeStorage>> GetAllByTypeStorageId(Guid ID);
}
