using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Ardita.Repositories.Classess
{
    public class TemplateSettingRepository : ITemplateSettingRepository
    {
        private readonly BksArditaDevContext _context;

        private readonly ILogChangesRepository _logChangesRepository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public TemplateSettingRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository,
            IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<int> Delete(MstTemplateSetting model)
        {
            int result = 0;

            var data = await _context.MstTemplateSettings.AsNoTracking().FirstAsync(x => x.TemplateSettingId == model.TemplateSettingId);
            if (data != null)
            {
                _context.MstTemplateSettings.Update(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstTemplateSetting>(GlobalConst.Delete, (Guid)model!.UpdatedBy!, new List<MstTemplateSetting> { data }, new List<MstTemplateSetting> { });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstTemplateSetting>> GetAll(string par = " 1=1 ")
        {
            return await _context.MstTemplateSettings.Include(x => x.MstTemplateSettingDetails)
            .Where(par).AsNoTracking().ToListAsync();
        }
        public List<string> GetListView()
        {
            return _context.Model.GetEntityTypes()
            .Select(t => t.GetViewName())
            .ToList()!;
        }
        public List<string> GetListColumnByViewName(string viewName)
        {
            var entityType = _context.Model.GetEntityTypes().ToList();

            List<string> result = new();
            if(!string.IsNullOrEmpty(viewName))
            {
                foreach (var item in entityType)
                {
                    if (item.GetViewName() == viewName)
                    {
                        result = item.GetProperties().Select(p => p.Name).ToList();
                        break;
                    }
                }
            }

            return result;
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.MstTemplateSettings
                .Where(x => x.TemplateName.ToLower().Contains(model.searchValue.ToLower()))
                .Where(x => (bool)model.IsArchiveActive! ? x.TemplateType == "Berita Acara" : x.TemplateType == "Label")
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.TemplateSettingId,
                    x.TemplateType,
                    x.TemplateName,
                    x.CreatedBy,
                    x.CreatedDate,
                    x.UpdatedBy,
                    x.UpdatedDate
                })
                .ToListAsync();

            return result;
        }

        public async Task<MstTemplateSetting> GetById(Guid id)
        {
            return await _context.MstTemplateSettings.Include(x => x.MstTemplateSettingDetails).AsNoTracking().FirstOrDefaultAsync(x => x.TemplateSettingId == id);
        }
        public async Task<DataTable> GetDataView(string viewName, Guid Id)
        {
            await Task.Delay(0);
            DataTable data = new();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand($"Select * from {viewName} where ID='{Id}'", connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(data);
                }
            }

            return data;
        }

        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var result = await _context.MstTemplateSettings
                .Where(x => x.TemplateName.ToLower().Contains(model.searchValue.ToLower()))
                .Where(x => (bool)model.IsArchiveActive! ? x.TemplateType == "Berita Acara" : true)
                .CountAsync();

            return result;
        }

        public async Task<int> Insert(MstTemplateSetting model, IFormFile file, IEnumerable<MstTemplateSettingDetail> detail)
        {
            int result = 0;

            if (model != null)
            {
                if(file != null)
                {
                    if (file.Length > 0)
                    {
                        Byte[] bytes;
                        using (var stream = new MemoryStream())
                        {
                            file.CopyTo(stream);
                            bytes = stream.ToArray();

                        }
                        string filename = model.TemplateName.Replace(" ", "") + Path.GetExtension(file.FileName);
                        string path = Path.Combine(_hostingEnvironment.WebRootPath, filename);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        File.WriteAllBytes(path, bytes);
                        model.Path = filename;
                    }
                }
                model.TemplateSettingId = Guid.NewGuid();
                _context.MstTemplateSettings.Add(model);
                result = await _context.SaveChangesAsync();

                List<MstTemplateSettingDetail> newDetail = new();
                if(detail.Count() > 0)
                {
                    foreach(var item in detail)
                    {
                        item.TemplateSettingId = model.TemplateSettingId;
                        item.CreatedBy = model.CreatedBy;
                        item.CreatedDate = model.CreatedDate;
                        newDetail.Add(item);
                    }

                    await _context.MstTemplateSettingDetails.AddRangeAsync(newDetail);
                    await _context.SaveChangesAsync();
                }

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstTemplateSetting>(GlobalConst.New, (Guid)model!.CreatedBy!, new List<MstTemplateSetting> { }, new List<MstTemplateSetting> { model });
                        await _logChangesRepository.CreateLog<MstTemplateSettingDetail>(GlobalConst.New, (Guid)model!.CreatedBy!, new List<MstTemplateSettingDetail> { }, newDetail);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<bool> InsertBulk(List<MstTemplateSetting> MstTemplateSettings)
        {
            bool result = false;
            if (MstTemplateSettings.Count() > 0)
            {
                await _context.AddRangeAsync(MstTemplateSettings);
                await _context.SaveChangesAsync();
                result = true;

                //Log
                if (result)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstTemplateSetting>(GlobalConst.New, (Guid)MstTemplateSettings.FirstOrDefault()!.CreatedBy!, new List<MstTemplateSetting> { }, MstTemplateSettings);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<int> Update(MstTemplateSetting model, IFormFile file, IEnumerable<MstTemplateSettingDetail> detail)
        {
            int result = 0;

            if (model != null && model.TemplateSettingId != Guid.Empty)
            {
                var data = await _context.MstTemplateSettings.AsNoTracking().FirstAsync(x => x.TemplateSettingId == model.TemplateSettingId);
                if (data != null)
                {
                    if (file != null)
                    {
                        if (file.Length > 0)
                        {
                            Byte[] bytes;
                            using (var stream = new MemoryStream())
                            {
                                file.CopyTo(stream);
                                bytes = stream.ToArray();

                            }
                            string filename = model.TemplateName.Replace(" ", "") + Path.GetExtension(file.FileName);
                            string path = Path.Combine(_hostingEnvironment.WebRootPath, filename);
                            if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                            File.WriteAllBytes(path, bytes);
                            model.Path = filename;

                            _context.MstTemplateSettings.Update(model);
                            result = await _context.SaveChangesAsync();
                        }
                    }

                    var oldDetail = await _context.MstTemplateSettingDetails.Where(x => x.TemplateSettingId == model.TemplateSettingId).ToListAsync();
                    if (oldDetail.Count > 0)
                    {
                        _context.MstTemplateSettingDetails.RemoveRange(oldDetail);
                        await _context.SaveChangesAsync();
                    }

                    List<MstTemplateSettingDetail> newDetail = new();
                    if (detail.Count() > 0)
                    {
                        foreach (var item in detail)
                        {
                            item.TemplateSettingId = model.TemplateSettingId;
                            item.CreatedBy = model.UpdatedBy;
                            item.CreatedDate = model.UpdatedDate;
                            newDetail.Add(item);
                        }

                        await _context.MstTemplateSettingDetails.AddRangeAsync(newDetail);
                        await _context.SaveChangesAsync();
                    }

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstTemplateSetting>(GlobalConst.Update, (Guid)model!.UpdatedBy!, new List<MstTemplateSetting> { data }, new List<MstTemplateSetting> { model });
                            await _logChangesRepository.CreateLog<MstTemplateSettingDetail>(GlobalConst.Update, (Guid)model!.UpdatedBy!, oldDetail, newDetail);
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
    }
}
