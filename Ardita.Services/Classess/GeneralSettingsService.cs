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
        List<IdxGeneralSettingsFormatFile> details = await SetDetail(listDetail);

        if (SiteLogo is not null)
        {
            SetSiteLogo(model, SiteLogo);
        }
        if (CompanyLogo is not null)
        {
            SetCompanyLogo(model, CompanyLogo);
        }
        if (FavIcon is not null)
        {
            SetFavIconLogo(model, FavIcon);
        }

        return await _repository.Insert(model, details);
    }

    public async Task<int> Update(MstGeneralSetting model, string[] listDetail, IFormFile SiteLogo, IFormFile CompanyLogo, IFormFile FavIcon)
    {
        List<IdxGeneralSettingsFormatFile> details = await SetDetail(listDetail);

        if (SiteLogo is not null)
        {
            SetSiteLogo(model, SiteLogo);
        }
        if (CompanyLogo is not null)
        {
            SetCompanyLogo(model, CompanyLogo);
        }
        if (FavIcon is not null)
        {
            SetFavIconLogo(model, FavIcon);
        }

        return await _repository.Update(model, details);
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

    private static void SetSiteLogo(MstGeneralSetting model, IFormFile SiteLogo)
    {
        FileModel file = new();

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
    }

    private static void SetCompanyLogo(MstGeneralSetting model, IFormFile CompanyLogo)
    {
        FileModel file = new();

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
    }

    private static void SetFavIconLogo(MstGeneralSetting model, IFormFile FavIcon)
    {
        FileModel file = new();

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
    }
    #endregion
}
