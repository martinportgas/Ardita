using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class FileArchiveDetailService : IFileArchiveDetailService
    {
        private readonly IFileArchiveDetailRepository _fileArchiveDetailRepository;

        public FileArchiveDetailService(IFileArchiveDetailRepository fileArchiveDetailRepository)
        {
            _fileArchiveDetailRepository = fileArchiveDetailRepository;
        }
        public Task<TrxFileArchiveDetail> GetById(Guid id)
        {
            return _fileArchiveDetailRepository.GetById(id);
        }
        public async Task<IEnumerable<TrxFileArchiveDetail>> GetAll()
        {
            return await _fileArchiveDetailRepository.GetAll();
        }
    }
}
