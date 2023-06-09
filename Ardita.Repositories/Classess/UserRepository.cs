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
    public class UserRepository : IUserRepository
    {
        private readonly BksArditaDevContext _context;
        public UserRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(MstUser model)
        {
            int result = 0;

            if (model.UserId != Guid.Empty)
            {
                var data = await _context.MstUsers.AsNoTracking().Where(x => x.UserId == model.UserId).ToListAsync();
                if (data != null)
                {
                    model.IsActive = false;
                    model.CreatedBy = data.FirstOrDefault().CreatedBy;
                    model.CreatedDate = data.FirstOrDefault().CreatedDate;
                    _context.MstUsers.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstUser>> GetAll()
        {
            var result = await _context.MstUsers.AsNoTracking().Where(x => x.IsActive == true).ToListAsync();
            return result;
        }

        public async Task<IEnumerable<MstUser>> GetById(Guid id)
        {
            var result = await _context.MstUsers.AsNoTracking().Where(x => x.UserId == id).ToListAsync();
            return result;
        }

        public async Task<int> Insert(MstUser model)
        {
            int result = 0;
            if (model != null)
            {
                var data = await _context.MstUsers.AsNoTracking().Where(x => x.EmployeeId == model.EmployeeId).ToListAsync();
                model.IsActive = true;

                if (data.Count > 0)
                {

                    model.UserId = data.FirstOrDefault().UserId;
                    model.UpdateBy = model.CreatedBy;
                    model.UpdateDate = DateTime.Now;
                    _context.MstUsers.Update(model);
                    result = await _context.SaveChangesAsync();
                }
                else
                {
                    _context.MstUsers.Add(model);
                    result = await _context.SaveChangesAsync();
                }
            }

            return result;
        }

        public async Task<bool> InsertBulk(List<MstUser> users)
        {
            bool result = false;
            if (users.Count() > 0)
            {
                await _context.BulkInsertAsync(users);
                result = true;
            }
            return result;
        }

        public async Task<int> Update(MstUser model)
        {
            int result = 0;

            if (model.UserId != Guid.Empty)
            {
                var data = await _context.MstUsers.AsNoTracking().Where(x => x.UserId == model.UserId).ToListAsync();
                if (data != null)
                {
                    model.IsActive = true;
                    model.CreatedBy = data.FirstOrDefault().CreatedBy;
                    model.CreatedDate = data.FirstOrDefault().CreatedDate;
                    _context.MstUsers.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
