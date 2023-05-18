using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class ArchiveRentService : IArchiveRentService
    {
        public Task<int> Delete(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrxArchiveRent>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TrxArchiveRent>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountByFilterModel(DataTableModel model)
        {
            throw new NotImplementedException();
        }

        public Task<int> Insert(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(TrxArchiveRent model)
        {
            throw new NotImplementedException();
        }
    }
}
