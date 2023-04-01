using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class PositionRepository : IPositionRepository
    {
        private readonly BksArditaDevContext _context;
        public PositionRepository(BksArditaDevContext context)
        {
            _context = context;
        }

        public Task<int> Delete(MstPosition model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MstPosition>> GetAll()
        {
            var results = await _context.MstPositions.ToListAsync();
            return results;
        }

        public async Task<IEnumerable<MstPosition>> GetById(Guid id)
        {
            var result = await _context.MstPositions.Where(x => x.PosittionId == id).ToListAsync();
            return result;
        }

        public async Task<int> Insert(MstPosition model)
        {
            int result = 0;

            if (model != null)
            {
                _context.MstPositions.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(MstPosition model)
        {
            int result = 0;

            if (model != null && model.PosittionId != Guid.Empty)
            {
                var data = await _context.MstPositions.Where(x => x.PosittionId == model.PosittionId).ToListAsync();
                if (data != null)
                {
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
