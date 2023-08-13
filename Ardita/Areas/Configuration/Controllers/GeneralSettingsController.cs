using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Configuration.Controllers;

[CustomAuthorize]
[Area(GlobalConst.Configuration)]
public class GeneralSettingsController : BaseController<MstGeneralSetting>
{
    #region MEMBER AND CTR
    public GeneralSettingsController(IGeneralSettingsService generalSettingsService) => GeneralSettingsService = generalSettingsService;
    #endregion

    public override async Task<ActionResult> Index() 
    {
        await Task.Delay(0);

        return RedirectToAction(GlobalConst.Add, GlobalConst.GeneralSettings, new { Area = GlobalConst.Configuration });
    }

    public override async Task<IActionResult> Add()
    {
        bool isExists = await GeneralSettingsService.IsExist();

        if (isExists)
        {
            var data = await GeneralSettingsService.GetExistingSettings();
            ViewBag.SiteLogo = Convert.FromBase64String(data.SiteLogoContent);
            return View(GlobalConst.Form, data);

        }

        return View(GlobalConst.Form, new MstGeneralSetting());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(MstGeneralSetting model, IFormFile SiteLogoContent, IFormFile CompanyLogoContent, IFormFile FavIconContent)
    {
        await Task.Delay(0);

        if (model != null)
        {
            var listDetail = Request.Form[GlobalConst.DetailArray].ToArray();

            if (model.GeneralSettingsId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                //await GeneralSettingsService.Update(model, listDetail!);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await GeneralSettingsService.Insert(model, listDetail!, SiteLogoContent, CompanyLogoContent, FavIconContent);
            }
        }
        return RedirectToIndex();
    }


    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Gmd, new { Area = GlobalConst.MasterData });
    #endregion
}
