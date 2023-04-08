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

        public async Task<int> Delete(MstPosition model)
        {
            int result = 0;

            if (model != null && model.PosittionId != Guid.Empty)
            {
                var data = await _context.MstPositions.AsNoTracking().Where(x => x.PosittionId == model.PosittionId).ToListAsync();
                if (data.Count > 0)
                {
                    model.IsActive = false;
                    _context.MstPositions.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstPosition>> GetAll()
        {
            var results = await _context.MstPositions.Where(x=>x.IsActive == true).ToListAsync();
            return results;
        }

        public async Task<IEnumerable<MstPosition>> GetById(Guid id)
        {
            var result = await _context.MstPositions.AsNoTracking().Where(x => x.PosittionId == id).ToListAsync();
            return result;
        }

        public async Task<int> Insert(MstPosition model)
        {
            int result = 0;

            if (model.PosittionId == Guid.Empty)
            {
                var data = await _context.MstPositions.AsNoTracking().Where(x => x.Code.ToUpper() == model.Code.ToUpper()).ToListAsync();
                model.IsActive = true;
                if (data.Count > 0)
                {
                    model.PosittionId = data.FirstOrDefault().PosittionId;
                    _context.MstPositions.Update(model);
                }
                else 
                {
                    _context.MstPositions.Add(model);
                }
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(MstPosition model)
        {
            int result = 0;

            if (model != null && model.PosittionId != Guid.Empty)
            {
                var data = await _context.MstPositions.AsNoTracking().Where(x => x.PosittionId == model.PosittionId).ToListAsync();
                if (data != null)
                {
                    model.IsActive = true;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
