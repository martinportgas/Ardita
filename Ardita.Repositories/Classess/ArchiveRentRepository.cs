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
       
        public ArchiveRentRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public async Task<int> Approval(Guid id, string description, int status, Guid User)
        {
            int result = 0;
            var data = await _context.TrxArchiveRents.FirstOrDefaultAsync(x => x.TrxArchiveRentId == id);
            if (data != null)
            {
                data.StatusId = status;
                if (status == 3)
                {
                    data.ApprovalDate = DateTime.Now;
                    data.ApprovalReturnDate = DateTime.Now.AddDays(7);
                    data.ApprovedBy = User;
                }
                else
                {
                    data.RejectedBy = User;
                }
               
                data.ApprovalNotes = description;
                data.UpdatedBy = User;
                data.UpdatedDate = DateTime.Now;

                _context.TrxArchiveRents.Update(data);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public Task<int> Delete(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrxArchiveRent>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<object>> GetApprovalByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveRents
                 .Include(x => x.User.Employee.Company)
                 .Include(x => x.Archive.Creator)
                 .Include(x => x.Status)
                 .Where(x => x.StatusId == 2)
                 .Where($"(User.Employee.Nik+User.Employee.Name).Contains(@0)", model.searchValue)
                 .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                 .Skip(model.skip).Take(model.pageSize)
                 .Select(x => new
                 {
                     x.TrxArchiveRentId,
                     EmployeeNik = x.User.Employee.Nik,
                     EmployeeName = x.User.Employee.Name,
                     ArchiveTitle = x.Archive.TitleArchive,
                     CompanyName = x.User.Employee.Company.CompanyName,
                     CreatorName = x.Archive.Creator.CreatorName
                 })
                 .ToListAsync();

            return result;
        }

        public async Task<int> GetApprovalCountByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveRents
               .Include(x => x.User.Employee.Company)
               .Include(x => x.Archive.Creator)
               .Where(x => x.StatusId == 2)
               .Where($"(User.Employee.Nik+User.Employee.Name).Contains(@0)", model.searchValue)
               .CountAsync();

            return result;
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
                    Status = x.Status.Name,
                    Color = x.Status.Color
                })
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<TrxArchiveRent>> GetById(Guid id)
        {
            var result = await _context.TrxArchiveRents.Where(x => x.TrxArchiveRentId == id && x.StatusId == 2).ToListAsync();
            return result;
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
