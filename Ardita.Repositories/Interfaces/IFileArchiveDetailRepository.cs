using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IFileArchiveDetailRepository
    {
        Task<IEnumerable<TrxFileArchiveDetail>> GetAll();
        Task<TrxFileArchiveDetail> GetById(Guid id);
        Task<IEnumerable<TrxFileArchiveDetail>> GetByArchiveId(Guid id);
    }
}
