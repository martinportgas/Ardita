using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class ArchiveOwnerService : IArchiveOwnerService
    {
        private readonly IArchiveOwnerRepository _archiveOwnerRepository;

        public ArchiveOwnerService(IArchiveOwnerRepository archiveOwnerRepository) => _archiveOwnerRepository = archiveOwnerRepository;
        public Task<int> Delete(MstArchiveOwner model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MstArchiveOwner>> GetAll()
        {
            return await _archiveOwnerRepository.GetAll();
        }

        public Task<IEnumerable<MstArchiveOwner>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DataTableResponseModel<MstArchiveOwner>> GetList(DataTablePostModel model)
        {
            throw new NotImplementedException();
        }

        public Task<int> Insert(MstArchiveOwner model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertBulk(List<MstArchiveOwner> MstArchiveOwners)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(MstArchiveOwner model)
        {
            throw new NotImplementedException();
        }
    }
}
