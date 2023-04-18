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
        int result = 0;
        if (model != null)
        {

            if (model.CompanyId != Guid.Empty)
            {
                model.UpdatedBy = new Guid(User.FindFirst(Const.UserId).Value);
                model.UpdatedDate = DateTime.Now;
                result = await _companyService.Update(model);
            }
            else
            {
                model.CreatedBy = new Guid(User.FindFirst(Const.UserId).Value);
                model.CreatedDate = DateTime.Now;
                result = await _companyService.Insert(model);
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
        int result = 0;
        if (model != null)
        {
            model.UpdatedBy = new Guid(User.FindFirst(Const.UserId).Value);
            model.UpdatedDate = DateTime.Now;
            result = await _companyService.Delete(model);

        }
        return RedirectToIndex();
    }

    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Company, new { Area = Const.MasterData });

}
