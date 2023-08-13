using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Ardita.Services.Classess;

public class GeneralSettingsService : IGeneralSettingsService
{
    private readonly IGeneralSettingsRepository _repository;

    public GeneralSettingsService(IGeneralSettingsRepository generalSettingsRepository)
    {
            _repository = generalSettingsRepository;
    }

    public async Task<int> Insert(MstGeneralSetting model, string[] listDetail, IFormFile SiteLogo, IFormFile CompanyLogo, IFormFile FavIcon)
    {
        MstGeneralSetting data = new();
        List<IdxGeneralSettingsFormatFile> details = await SetDetail(listDetail);

        data = await SetSiteLogo(model, SiteLogo);
        data = await SetCompanyLogo(model, CompanyLogo);
        data = await SetFavIconLogo(model, FavIcon);

        return await _repository.Insert(data, details);
    }

    public Task<bool> IsExist()=> _repository.IsExist();

    public async Task<MstGeneralSetting> GetExistingSettings()
    {
        return await _repository.GetExistingSettings();
    }


    #region SETUP
    private static async Task<List<IdxGeneralSettingsFormatFile>> SetDetail(string[] listDetail)
    {
        List<IdxGeneralSettingsFormatFile> details = new();
        await Task.Delay(0);

        foreach (var item in listDetail)
        {
            IdxGeneralSettingsFormatFile detail = new()
            {
                FormatFileName = item
            };

            details.Add(detail);
        }

        return details;
    }

    private static async Task<MstGeneralSetting> SetSiteLogo(MstGeneralSetting model, IFormFile SiteLogo)
    {
        FileModel file = new();
        await Task.Delay(0);

        using (var memoryStream = new MemoryStream())
        {
            SiteLogo.CopyTo(memoryStream);

            file.FileName = SiteLogo.FileName;
            file.FileType = SiteLogo.ContentType;
            file.Content = memoryStream.ToArray();
        }

        model.SiteLogoContent = Convert.ToBase64String(file.Content!);
        model.SiteLogoFileName = file.FileName!;
        model.SiteLogoFileType = file.FileType!;

        return model;
    }

    private static async Task<MstGeneralSetting> SetCompanyLogo(MstGeneralSetting model, IFormFile CompanyLogo)
    {
        FileModel file = new();
        await Task.Delay(0);

        using (var memoryStream = new MemoryStream())
        {
            CompanyLogo.CopyTo(memoryStream);

            file.FileName = CompanyLogo.FileName;
            file.FileType = CompanyLogo.ContentType;
            file.Content = memoryStream.ToArray();
        }

        model.CompanyLogoContent = Convert.ToBase64String(file.Content!);
        model.CompanyLogoFileName = file.FileName!;
        model.CompanyLogoFileType = file.FileType!;

        return model;
    }

    private static async Task<MstGeneralSetting> SetFavIconLogo(MstGeneralSetting model, IFormFile FavIcon)
    {
        FileModel file = new();
        await Task.Delay(0);

        using (var memoryStream = new MemoryStream())
        {
            FavIcon.CopyTo(memoryStream);

            file.FileName = FavIcon.FileName;
            file.FileType = FavIcon.ContentType;
            file.Content = memoryStream.ToArray();
        }

        model.FavIconContent = Convert.ToBase64String(file.Content!);
        model.FavIconFileName = file.FileName!;
        model.FavIconFileType = file.FileType!;

        return model;
    }
    #endregion
}
