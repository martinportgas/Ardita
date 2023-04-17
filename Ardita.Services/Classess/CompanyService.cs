using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Companies;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<int> Delete(MstCompany model)
    {
        return await _companyRepository.Delete(model);
    }

    public async Task<IEnumerable<MstCompany>> GetAll()
    {
        return await _companyRepository.GetAll();
    }

    public async Task<IEnumerable<MstCompany>> GetById(Guid id)
    {
        return await _companyRepository.GetById(id);
    }

    public async Task<CompanyListViewModel> GetListCompanies(DataTableModel tableModel)
    {
        List<CompanyListViewDetailModel>? data;
        var companyListViewModel = new CompanyListViewModel();
        var companyResult = await _companyRepository.GetAll();

        var results = from company in companyResult
                      select new CompanyListViewDetailModel
                      {
                          CompanyId = company.CompanyId,
                          CompanyCode = company.CompanyCode,
                          CompanyName = company.CompanyName,
                          Address = company.Address,
                          Telepone = company.Telepone,
                          Email = company.Email
                      };
        if (!string.IsNullOrEmpty(tableModel.searchValue))
        {
            results = results.Where(
                    x =>
                    (x.CompanyCode != null ? x.CompanyCode.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.CompanyName != null ? x.CompanyName.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.Address != null ? x.Address.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.Telepone != null ? x.Telepone.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.Email != null ? x.Email.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                );
        }

        tableModel.recordsTotal = results.Count();
        data = results.Skip(tableModel.skip).Take(tableModel.pageSize).ToList();

        companyListViewModel.Draw = tableModel.draw;
        companyListViewModel.RecordsFiltered = tableModel.recordsTotal;
        companyListViewModel.RecordsTotal = tableModel.recordsTotal;
        companyListViewModel.Data = data;

        return companyListViewModel;
    }

    public async Task<int> Insert(MstCompany model)
    {
        return await _companyRepository.Insert(model);
    }

    public async Task<int> Update(MstCompany model)
    {
        return await _companyRepository.Update(model);
    }
}
