using Ardita.Models.DbModels;
using Microsoft.AspNetCore.Http;

namespace Ardita.Services.Interfaces;

public interface IGeneralSettingsService
{
    Task<int> Insert(MstGeneralSetting model, string[] listDetail, IFormFile SiteLogo, IFormFile CompanyLogo, IFormFile FavIcon);
    Task<bool> IsExist();
    Task<MstGeneralSetting> GetExistingSettings();
}
