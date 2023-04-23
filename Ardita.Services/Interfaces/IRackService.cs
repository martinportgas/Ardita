using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IRackService
    {
        Task<IEnumerable<TrxRack>> GetById(Guid id);
        Task<IEnumerable<TrxRack>> GetAll();
        Task<DataTableResponseModel<TrxRack>> GetListClassification(DataTablePostModel model);
        Task<int> Insert(TrxRack model);
        Task<int> Delete(TrxRack model);
        Task<int> Update(TrxRack model);
    }
}
