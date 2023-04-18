using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<MstCompany>> GetById(Guid id);
        Task<IEnumerable<MstCompany>> GetAll();
        Task<IEnumerable<MstCompany>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(MstCompany model);
        Task<int> Delete(MstCompany model);
        Task<int> Update(MstCompany model);
    }
}
