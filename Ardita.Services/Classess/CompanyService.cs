using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Companies;
using Ardita.Repositories.Classess;
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

    public async Task<DataTableResponseModel<MstCompany>> GetListCompanies(DataTablePostModel model)
    {
        try
        {
            var dataCount = await _companyRepository.GetCount();

            var filterData = new DataTableModel();

            filterData.sortColumn = model.columns[model.order[0].column].data;
            filterData.sortColumnDirection = model.order[0].dir;
            filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
            filterData.pageSize = model.length;
            filterData.skip = model.start;

            var results = await _companyRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<MstCompany>();

            responseModel.draw = model.draw;
            responseModel.recordsTotal = dataCount;
            responseModel.recordsFiltered = dataCount;
            responseModel.data = results.ToList();

            return responseModel;
        }
        catch (Exception ex)
        {
            return null;
        }

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
