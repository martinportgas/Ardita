using Ardita.Models.DbModels;

namespace Ardita.Repositories.Interfaces
{
    public interface IUserArchiveUnitRepository
    {
        Task<IEnumerable<IdxUserArchiveUnit>> GetById(Guid id);
        Task<IEnumerable<IdxUserArchiveUnit>> GetByUserId(Guid id);
        Task<IEnumerable<IdxUserArchiveUnit>> GetAll(string par = " 1=1 ");
        Task<int> Insert(IdxUserArchiveUnit model);
        Task<bool> InsertBulk(List<IdxUserArchiveUnit> models);
        Task<int> Delete(IdxUserArchiveUnit model);
        Task<int> DeleteByUserId(Guid Id);
        Task<int> Update(IdxUserArchiveUnit model);
    }
}
