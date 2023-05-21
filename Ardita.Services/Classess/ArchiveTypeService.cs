using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class ArchiveTypeService : IArchiveTypeService
    {
        private readonly IArchiveTypeRepository _archiveTypeRepository;
        public ArchiveTypeService(IArchiveTypeRepository archiveTypeRepository) => _archiveTypeRepository = archiveTypeRepository;
        public Task<int> Delete(MstArchiveType model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MstArchiveType>> GetAll()
        {
            return await _archiveTypeRepository.GetAll();
        }

        public Task<IEnumerable<MstArchiveType>> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<DataTableResponseModel<MstArchiveType>> GetList(DataTablePostModel model)
        {
            throw new NotImplementedException();
        }

        public Task<int> Insert(MstArchiveType model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertBulk(List<MstArchiveType> MstArchiveTypes)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(MstArchiveType model)
        {
            throw new NotImplementedException();
        }
    }
}
