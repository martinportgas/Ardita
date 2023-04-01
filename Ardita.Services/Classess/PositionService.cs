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
    public class PositionService : IPositionService
    {
        private readonly IPositionRepository _positionRepository;
        public PositionService(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<int> Delete(MstPosition model)
        {
            return await _positionRepository.Delete(model);
        }

        public async Task<IEnumerable<MstPosition>> GetAll()
        {
            return await _positionRepository.GetAll();
        }

        public async Task<IEnumerable<MstPosition>> GetById(Guid id)
        {
            return await _positionRepository.GetById(id);
        }

        public async Task<int> Insert(MstPosition model)
        {
            return await _positionRepository.Insert(model);
        }

        public async Task<int> Update(MstPosition model)
        {
            return await _positionRepository.Update(model);
        }
    }
}
