using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess
{
    public class ArchiveRentRepository : IArchiveRentRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly string _whereClause = @"TitleArchive+TypeSender+Keyword+ActiveRetention.ToString()+CreatedDateArchive.ToString()
                                            +InactiveRetention.ToString()+Volume.ToString()+Gmd.GmdName+SubSubjectClassification.SubSubjectClassificationName
                                            +Creator.CreatorName+TypeArchive";
        public ArchiveRentRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public Task<int> Delete(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrxArchiveRent>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveRents
                .Include(x => x.User.Employee)
                .Include(x => x.Archive)
                .Include(x => x.Status)
                .Where($"(User.Employee.Name+RequestedDate.ToString()+RequestedReturnDate.ToString()).Contains(@0)", model.searchValue)
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new
                {
                    x.TrxArchiveRentId,
                    x.User.Employee.Name,
                    x.RequestedDate,
                    x.RequestedReturnDate,
                    x.StatusId,
                    Status = x.Status.Name
                })
                .ToListAsync();

            return result;
        }

        public Task<IEnumerable<TrxArchiveRent>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveRents
               .Include(x => x.User.Employee)
               .Include(x => x.Archive)
               .Include(x => x.Status)
               .Where($"(User.Employee.Name+RequestedDate.ToString()+ReturnDate.ToString()).Contains(@0)", model.searchValue)
               .CountAsync();

            return result;
        }

        public async Task<int> Insert(TrxArchiveRent model)
        {
            int result = 0;

            if (model != null)
            {
                model.StatusId = (int)GlobalConst.STATUS.ApprovalProcess;
                
                _context.TrxArchiveRents.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public Task<int> Update(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }
    }
}
