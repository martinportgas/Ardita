﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class ArchiveRepository : IArchiveRepository
{
    private readonly BksArditaDevContext _context;
    private readonly IConfiguration _configuration;

    public ArchiveRepository(BksArditaDevContext context, IConfiguration configuration) 
    {
        _context = context;
        _configuration = configuration;
    }

    public Task<int> Delete(TrxArchive model)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TrxArchive>> GetAll()
    {
        return await _context.TrxArchives.Where(x => x.IsActive == true).ToListAsync();
    }

    public async Task<IEnumerable<TrxArchive>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<TrxArchive> result;

        var propertyInfo = typeof(TrxArchive).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(TrxArchive).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SubSubjectClassification)
                .Include(x => x.Creator)
                .Where(x => (x.TitleArchive).Contains(model.searchValue) && x.IsActive == true)
                .OrderBy(x => EF.Property<TrxArchive>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }
        else
        {
            result = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SubSubjectClassification)
                .Include(x => x.Creator)
                .Where(x => (x.TitleArchive).Contains(model.searchValue) && x.IsActive == true)
                .OrderByDescending(x => EF.Property<TrxArchive>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }

        return result;
    }

    public async Task<IEnumerable<TrxArchive>> GetByFilterModelForMonitoring(DataTableModel model)
    {
        IEnumerable<TrxArchive> result;

        var propertyInfo = typeof(TrxArchive).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(TrxArchive).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SubSubjectClassification)
                .Include(x => x.Creator)
                .Where(x => (x.TitleArchive).Contains(model.searchValue) && x.IsActive == true && x.SubSubjectClassification.TrxPermissionClassifications.FirstOrDefault().PositionId == model.PositionId)
                .OrderBy(x => EF.Property<TrxArchive>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }
        else
        {
            result = await _context.TrxArchives
                .Include(x => x.Gmd)
                .Include(x => x.SubSubjectClassification)
                .Include(x => x.Creator)
                .Where(x => (x.TitleArchive).Contains(model.searchValue) && x.IsActive == true && x.SubSubjectClassification.TrxPermissionClassifications.FirstOrDefault().PositionId == model.PositionId)
                .OrderByDescending(x => EF.Property<TrxArchive>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }

        return result;
    }

    public async Task<TrxArchive> GetById(Guid id)
    {
        var result = await _context.TrxArchives.AsNoTracking()
            .Include(x => x.Gmd)
            .Include(x => x.SubSubjectClassification)
            .Include(x => x.SecurityClassification)
            .Include(x => x.Creator)
            .Include(x => x.TrxFileArchiveDetails)
            .Include(x => x.TrxMediaStorageDetails)
            .ThenInclude(x => x.MediaStorage)
            .ThenInclude(x => x.Row)
            .ThenInclude(x => x.Level)
            .ThenInclude(x => x.Rack)
            .ThenInclude(x => x.Room)
            .ThenInclude(x => x.Floor)
            .ThenInclude(x => x.ArchiveUnit)
            .Include(x => x.TrxMediaStorageDetails)
            .ThenInclude(x => x.MediaStorage)
            .ThenInclude(x => x.TypeStorage)

            .Where(x => x.ArchiveId == id && x.IsActive == true)
            .FirstAsync();

        

        return result;
    }

    public async Task<int> GetCount() => await _context.TrxArchives.CountAsync(x => x.IsActive == true);
    public async Task<int> GetCountForMonitoring(Guid? PositionId)
    {
        return await _context
             .TrxArchives
             .Include(x => x.Gmd)
             .Include(x => x.SubSubjectClassification)
             .Include(x => x.Creator)
             .CountAsync(x => x.IsActive == true
             && x.SubSubjectClassification.TrxPermissionClassifications.FirstOrDefault().PositionId == PositionId);
    }

    public async Task<int> Insert(TrxArchive model, List<FileModel> files)
    {
        int result = 0;
        var directory = _configuration["Path:Archive"];

        if (model != null)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                model.IsActive = true;
                _context.TrxArchives.Add(model);
                result = await _context.SaveChangesAsync();

                if (result != 0 && files.Any())
                {
                    foreach (FileModel file in files)
                    {
                        TrxFileArchiveDetail temp = new()
                        {
                            ArchiveId = model.ArchiveId,
                            FileName = file.FileName!,
                            FilePath = directory + model.ArchiveId.ToString() + "\\" + file.FileName,
                            FileType = file.FileType!,
                            CreatedBy = model.CreatedBy,
                            CreatedDate = model.CreatedDate,
                            IsActive = true
                        };

                        _context.TrxFileArchiveDetails.Add(temp);
                        directory = directory + model.ArchiveId.ToString();

                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
    

                        Byte[] bytes = Convert.FromBase64String(file.Base64!);
                        File.WriteAllBytes(temp.FilePath, bytes);
                    }
                    await _context.SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        return result;
    }

    public Task<int> Update(TrxArchive model)
    {
        throw new NotImplementedException();
    }
}