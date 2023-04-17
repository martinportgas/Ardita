using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            int result = 0;

            if (model.CompanyId != Guid.Empty)
            {
                var data = await _context.MstCompanies.AsNoTracking().Where(x => x.CompanyId == model.CompanyId).ToListAsync();
                if (data != null)
                {
                    model.IsActive = false;
                    model.CreatedBy = data.FirstOrDefault().CreatedBy;
                    model.CreatedDate = data.FirstOrDefault().CreatedDate;
                    _context.MstCompanies.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstCompany>> GetAll()
        {
            var results = await _context.MstCompanies.Where(x => x.IsActive == true).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<MstCompany>> GetById(Guid id)
        {
            var results = await _context.MstCompanies.Where(x => x.CompanyId == id).ToListAsync();
            return results;
        }

        public async Task<int> Insert(MstCompany model)
        {
            int result = 0;
            if (model != null)
            {
                var data = await _context.MstCompanies.AsNoTracking().Where(x => x.CompanyId == model.CompanyId).ToListAsync();
                model.IsActive = true;

                if (data.Count > 0)
                {

                    model.CompanyId = data.FirstOrDefault().CompanyId;
                    model.UpdatedBy = model.CreatedBy;
                    model.UpdatedDate = DateTime.Now;
                    _context.MstCompanies.Update(model);
                    result = await _context.SaveChangesAsync();
                }
                else
                {
                    _context.MstCompanies.Add(model);
                    result = await _context.SaveChangesAsync();
                }
            }

            return result;
        }

        public async Task<int> Update(MstCompany model)
        {
            int result = 0;

            if (model != null && model.CompanyId != null)
            {
                var data = await _context.MstCompanies.AsNoTracking().Where(x => x.CompanyId == model.CompanyId).ToListAsync();
                if (data != null)
                {
                    model.IsActive = true;
                    model.CreatedBy = data.FirstOrDefault().CreatedBy;
                    model.CreatedDate = data.FirstOrDefault().CreatedDate;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
