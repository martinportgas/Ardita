using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly BksArditaDevContext _context;

        public CompanyRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public async Task<int> Delete(MstCompany model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MstCompany>> GetAll()
        {
            var results = await _context.MstCompanies.ToListAsync();
            return results;
        }

        public async Task<IEnumerable<MstCompany>> GetById(int id)
        {
            var results = await _context.MstCompanies.Where(x=> x.CompanyId == id).ToListAsync();
            return results;
        }

        public async Task<int> Insert(MstCompany model)
        {
            int result = 0;

            if (model != null)
            {
                _context.MstCompanies.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;

        }

        public async Task<int> Update(MstCompany model)
        {
            int result = 0;

            if (model != null && model.CompanyId != null)
            {
                var data = await _context.MstCompanies.Where(x => x.CompanyId == model.CompanyId).ToListAsync();
                if (data != null)
                {
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
