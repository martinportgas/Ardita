using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<MstUser>> GetById(Guid id);
        Task<IEnumerable<MstUser>> GetAll();
        Task<int> Insert(MstUser model);
        Task<int> Delete(MstUser model);
        Task<int> Update(MstUser model);
        void Upload(MstUser model);
    }
}
