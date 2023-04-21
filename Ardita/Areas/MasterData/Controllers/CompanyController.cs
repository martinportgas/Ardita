using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area(Const.MasterData)]
public class CompanyController : Controller
{
    private IWebHostEnvironment _webHostEnvironment;
    private readonly ICompanyService _companyService;

    public CompanyController(IWebHostEnvironment webHostEnvironment, ICompanyService companyService)
    {
        _webHostEnvironment = webHostEnvironment;
        _companyService = companyService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _companyService.GetListCompanies(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IActionResult> Add()
    {
        var Company = new MstCompany();
        ViewBag.CurrentAction = Const.Add;

        await Task.Delay(0);

        return View(Const.Form, Company);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(MstCompany model)
    {
        if (model != null)
        {

            if (model.CompanyId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _companyService.Update(model);
            }
            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _companyService.Insert(model);
            }

        }
        return RedirectToIndex();
    }

    public async Task<IActionResult> Update(Guid Id)
    {
        var listCompany = await _companyService.GetById(Id);

        if (listCompany.Any())
        {
            return View(Const.Form, listCompany.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public async Task<IActionResult> Detail(Guid Id)
    {
        var listCompany = await _companyService.GetById(Id);

        if (listCompany.Any())
        {
            return View(Const.Form, listCompany.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public async Task<IActionResult> Remove(Guid Id)
    {
        var listCompany = await _companyService.GetById(Id);

        if (listCompany.Any())
        {
            return View(Const.Form, listCompany.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public async Task<IActionResult> Delete(MstCompany model)
    {
        if (model != null)
        {
            model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
            model.UpdatedDate = DateTime.Now;
            await _companyService.Delete(model);

        }
        return RedirectToIndex();
    }

    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Company, new { Area = Const.MasterData });

}
