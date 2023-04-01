using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
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

        public async Task<IEnumerable<MstCompany>> GetById(int id)
        {
            return await _companyRepository.GetById(id);
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
}
