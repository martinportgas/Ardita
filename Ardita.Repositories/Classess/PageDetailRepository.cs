﻿using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class PageDetailRepository : IPageDetailRepository
    {
        private readonly BksArditaDevContext _context;
        public PageDetailRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(MstPageDetail model)
        {
            int result = 0;

            if (model.PageDetailId != Guid.Empty)
            {
                _context.MstPageDetails.Remove(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> DeleteByMainId(Guid id)
        {
            int result = 0;
            if (id != null)
            {
                _context.Database.ExecuteSqlRaw($" delete from dbo.MST_PAGE_DETAIL where page_id='{id}'");
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<MstPageDetail>> GetAll()
        {
            var results = await _context.MstPageDetails.ToListAsync();
            return results;
        }

        public async Task<IEnumerable<MstPageDetail>> GetById(Guid id)
        {
            var result = await _context.MstPageDetails.AsNoTracking().Where(x => x.PageDetailId == id).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MstPageDetail>> GetByMainId(Guid id)
        {
            var result = await _context.MstPageDetails.AsNoTracking().Where(x => x.PageId == id).OrderBy(x => x.CreatedDate).ToListAsync();
            return result;
        }

        public async Task<int> Insert(MstPageDetail model)
        {
            int result = 0;

            if (model != null)
            {
                var data = await _context.MstPageDetails.AsNoTracking().Where(
                  x => x.PageId == model.PageId &&
                  x.Path == model.Path
                  ).ToListAsync();

                if (data.Count() == 0)
                {
                    _context.MstPageDetails.Add(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<int> Update(MstPageDetail model)
        {
            int result = 0;

            if (model != null && model.PageDetailId != Guid.Empty)
            {
                var data = await _context.MstPageDetails.AsNoTracking().Where(x => x.PageDetailId == model.PageDetailId).ToListAsync();
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