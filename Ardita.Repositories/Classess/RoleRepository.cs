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
    public class RoleRepository : IRoleRepository
    {
        private readonly BksArditaDevContext _context;
        public RoleRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(MstRole model)
        {
            int result = 0;

            if (model.RoleId != Guid.Empty)
            {
                var data = await _context.MstRoles.AsNoTracking().Where(x => x.RoleId == model.RoleId).ToListAsync();
                if (data != null)
                {
                    model.IsActive = false;
                    model.CreatedBy = data.FirstOrDefault().CreatedBy;
                    model.CreatedDate = data.FirstOrDefault().CreatedDate;
                    _context.MstRoles.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstRole>> GetAll()
        {
            var result = await _context.MstRoles.Where(x => x.IsActive == true).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<MstRole>> GetById(Guid id)
        {
            
            var result = await _context.MstRoles.AsNoTracking().Where(x => x.RoleId == id).ToListAsync();
            return result;
        }
        public async Task<int> Insert(MstRole model)
        {
            int result = 0;
            if (model != null)
            {
                var data = await _context.MstRoles.AsNoTracking().Where(x => x.Code == model.Code).ToListAsync();
                model.IsActive = true;

                if (data.Count > 0)
                {
                    model.RoleId = data.FirstOrDefault().RoleId;
                    model.UpdateBy = model.CreatedBy;
                    model.UpdateDate = DateTime.Now;
                    _context.MstRoles.Update(model);
                    result = await _context.SaveChangesAsync();
                }
                else
                {
                    _context.MstRoles.Add(model);
                    result = await _context.SaveChangesAsync();
                }
            }

            return result;
        }

        public async Task<int> Update(MstRole model)
        {
            int result = 0;

            if (model.RoleId != Guid.Empty)
            {
                var data = await _context.MstRoles.AsNoTracking().Where(x => x.RoleId == model.RoleId).ToListAsync();
                if (data != null)
                {
                    model.IsActive = true;
                    model.CreatedBy = data.FirstOrDefault().CreatedBy;
                    model.CreatedDate = data.FirstOrDefault().CreatedDate;
                    _context.MstRoles.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
