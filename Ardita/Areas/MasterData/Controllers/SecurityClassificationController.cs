using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area(Const.MasterData)]
public class SecurityClassificationController : BaseController<MstSecurityClassification>
{

    #region MEMBER AND CTR

    public SecurityClassificationController(ISecurityClassificationService securityClassificationService) => _securityClassificationService = securityClassificationService;
    #endregion

    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _securityClassificationService.GetList(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public override async Task<IActionResult> Add()
    {
        await Task.Delay(0);
        return View(Const.Form, new MstSecurityClassification());
    }

    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _securityClassificationService.GetById(Id);
        if (data.Any())
        {
            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _securityClassificationService.GetById(Id);
        if (data.Any())
        {
            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _securityClassificationService.GetById(Id);
        if (data.Any())
        {
            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(MstSecurityClassification model)
    {
        if (model != null)
        {
            if (model.SecurityClassificationId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _securityClassificationService.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _securityClassificationService.Insert(model);
            }
        }
        return RedirectToIndex();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(MstSecurityClassification model)
    {
        if (model != null && model.SecurityClassificationId != Guid.Empty)
        {
            await _securityClassificationService.Delete(model);
        }
        return RedirectToIndex();
    }
    #endregion

    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.SecurityClassification, new { Area = Const.MasterData });
    #endregion
}
