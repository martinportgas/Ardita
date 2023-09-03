using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Positions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IPositionService
    {
        Task<MstPosition> GetById(Guid id);
        Task<IEnumerable<MstPosition>> GetAll(string par = " 1=1 ");
        Task<DataTableResponseModel<object>> GetListPositions(DataTablePostModel model);
        Task<int> Insert(MstPosition model);
        Task<int> Delete(MstPosition model);
        Task<int> Update(MstPosition model);
    }
}
