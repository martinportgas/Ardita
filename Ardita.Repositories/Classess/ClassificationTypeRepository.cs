using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess
{
    public class ClassificationTypeRepository : IClassificationTypeRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public ClassificationTypeRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }
        public async Task<int> Delete(MstTypeClassification model)
        {
            int result = 0;

            if (model.TypeClassificationId != Guid.Empty)
            {
                var data = _context.MstTypeClassifications.AsNoTracking().Where(x => x.TypeClassificationId == model.TypeClassificationId).FirstOrDefault();
                if (data != null)
                {
                    data.IsActive = false;
                    data.UpdatedBy = model.UpdatedBy;
                    data.UpdatedDate = DateTime.Now;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstTypeClassification>(GlobalConst.Delete, (Guid)model.UpdatedBy!, new List<MstTypeClassification> { data }, new List<MstTypeClassification> { });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstTypeClassification>> GetAll(string par = " 1=1 ")
        {
            var results = await _context.MstTypeClassifications
                .Where(x => x.IsActive == true)
                .Where(par)
                .AsNoTracking()
                .ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.MstTypeClassifications.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<MstTypeClassification> GetById(Guid id)
        {
            var result = await _context.MstTypeClassifications.AsNoTracking().FirstOrDefaultAsync(x => x.TypeClassificationId == id && x.IsActive == true);
            return result;
        }
        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.MstTypeClassifications
            .Where(x => x.IsActive == true && x.TypeClassificationName.Contains(model.searchValue))
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
            .Skip(model.skip).Take(model.pageSize)
            .Select(x => new {
                x.TypeClassificationId,
                x.TypeClassificationCode,
                x.TypeClassificationName
            })
            .ToListAsync();

            return result;
        }
        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var result = await _context.MstTypeClassifications
            .Where(x => x.IsActive == true && x.TypeClassificationName.Contains(model.searchValue))
            .CountAsync();

            return result;
        }

        public async Task<int> Insert(MstTypeClassification model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.MstTypeClassifications.Add(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstTypeClassification>(GlobalConst.New, (Guid)model.CreatedBy, new List<MstTypeClassification> {  }, new List<MstTypeClassification> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<MstTypeClassification> models)
        {
            bool result = false;
            if (models.Count() > 0)
            {
                await _context.AddRangeAsync(models);
                await _context.SaveChangesAsync();
                result = true;

                //Log
                if (result)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstTypeClassification>(GlobalConst.New, (Guid)models.FirstOrDefault()!.CreatedBy!, new List<MstTypeClassification> { }, models);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
        public async Task<int> Update(MstTypeClassification model)
        {
            int result = 0;

            if (model != null && model.TypeClassificationId != Guid.Empty)
            {
                var data = await _context.MstTypeClassifications.AsNoTracking().FirstOrDefaultAsync(x => x.TypeClassificationId == model.TypeClassificationId);
                if (data != null)
                {
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstTypeClassification>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<MstTypeClassification> { data }, new List<MstTypeClassification> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
    }
}
