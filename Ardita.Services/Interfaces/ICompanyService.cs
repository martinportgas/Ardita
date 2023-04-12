using Ardita.Models.DbModels;

namespace Ardita.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<MstCompany>> GetById(Guid id);
        Task<IEnumerable<MstCompany>> GetAll();
        Task<int> Insert(MstCompany model);
        Task<int> Delete(MstCompany model);
        Task<int> Update(MstCompany model);
    }
}
