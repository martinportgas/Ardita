using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Companies;

namespace Ardita.Services.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<MstCompany>> GetById(Guid id);
    Task<IEnumerable<MstCompany>> GetAll();
    Task<CompanyListViewModel> GetListCompanies(DataTableModel tableModel);
    Task<int> Insert(MstCompany model);
    Task<int> Delete(MstCompany model);
    Task<int> Update(MstCompany model);
}
