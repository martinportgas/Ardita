using Ardita.Models.DbModels;

namespace Ardita.Repositories.Interfaces
{
    public interface IUserArchiveUnitRepository
    {
        Task<IEnumerable<IdxUserArchiveUnit>> GetById(Guid id);
        Task<IEnumerable<IdxUserArchiveUnit>> GetAll();
        Task<int> Insert(IdxUserArchiveUnit model);
        Task<int> Delete(IdxUserArchiveUnit model);
        Task<int> Update(IdxUserArchiveUnit model);
    }
}
